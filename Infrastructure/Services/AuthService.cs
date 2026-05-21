using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstateHub.Application.DTOs.Auth;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Configurations;
using RealEstateHub.Infrastructure.Data;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AppDbContext dbContext,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwt = jwtOptions.Value;

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto, UserRole role, CancellationToken cancellationToken = default)
    {
        var roleName = role.ToString();
        if (role == UserRole.Admin)
        {
            return ApiResponse<AuthResponseDto>.Failure("Admin registration is not public.", 403);
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return ApiResponse<AuthResponseDto>.Failure("Registration failed.", 400, result.Errors.Select(x => x.Description).ToList());
        }

        await userManager.AddToRoleAsync(user, roleName);
        return ApiResponse<AuthResponseDto>.Success(await BuildAuthResponseAsync(user, roleName, cancellationToken), "Registered", 201);
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            return ApiResponse<AuthResponseDto>.Failure("Invalid email or password.", 401);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return ApiResponse<AuthResponseDto>.Failure("User is temporarily locked out.", 423);
        }

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
        {
            await userManager.AccessFailedAsync(user);
            return ApiResponse<AuthResponseDto>.Failure("Invalid email or password.", 401);
        }

        if (user.IsBlocked)
        {
            return ApiResponse<AuthResponseDto>.Failure("User is blocked.", 403);
        }

        await userManager.ResetAccessFailedCountAsync(user);
        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? UserRole.Buyer.ToString();
        return ApiResponse<AuthResponseDto>.Success(await BuildAuthResponseAsync(user, role, cancellationToken));
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default)
    {
        var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == dto.RefreshToken, cancellationToken);
        if (token is null || !token.IsActive)
        {
            return ApiResponse<AuthResponseDto>.Failure("Refresh token is invalid or expired.", 401);
        }

        var user = await userManager.FindByIdAsync(token.UserId);
        if (user is null || user.IsBlocked)
        {
            return ApiResponse<AuthResponseDto>.Failure("User is not available.", 401);
        }

        token.RevokedAt = DateTime.UtcNow;
        var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? UserRole.Buyer.ToString();
        return ApiResponse<AuthResponseDto>.Success(await BuildAuthResponseAsync(user, role, cancellationToken));
    }

    public async Task<ApiResponse<object>> LogoutAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default)
    {
        var token = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == dto.RefreshToken, cancellationToken);
        if (token is not null)
        {
            token.RevokedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return ApiResponse<object>.Success(null, "Logged out");
    }

    private async Task<AuthResponseDto> BuildAuthResponseAsync(AppUser user, string role, CancellationToken cancellationToken)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes);
        var accessToken = CreateAccessToken(user, role, expiresAt);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays)
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Role = role,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };
    }

    private string CreateAccessToken(AppUser user, string role, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Role, role),
            new("fullName", user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims, expires: expiresAt, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

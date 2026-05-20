using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Auth;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.WebAPI.Controllers;

public class AuthController(IAuthService authService) : ApiControllerBase
{
    [HttpPost("register/buyer")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterBuyer(RegisterDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await authService.RegisterAsync(dto, UserRole.Buyer, cancellationToken));
    }

    [HttpPost("register/seller")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterSeller(RegisterDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await authService.RegisterAsync(dto, UserRole.Seller, cancellationToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await authService.LoginAsync(dto, cancellationToken));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await authService.RefreshTokenAsync(dto, cancellationToken));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await authService.LogoutAsync(dto, cancellationToken));
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var data = new
        {
            UserId,
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        };

        return FromResponse(ApiResponse<object>.Success(data));
    }
}

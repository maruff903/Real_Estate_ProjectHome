using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Auth;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Enums;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register/buyer")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterBuyer(RegisterDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await authService.RegisterAsync(dto, UserRole.Buyer, cancellationToken));
    }

    [HttpPost("register/seller")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterSeller(RegisterDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await authService.RegisterAsync(dto, UserRole.Seller, cancellationToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await authService.LoginAsync(dto, cancellationToken));
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await authService.RefreshTokenAsync(dto, cancellationToken));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await authService.LogoutAsync(dto, cancellationToken));
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var data = new
        {
            UserId = User.GetUserId(),
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        };

        return this.FromResponse(ApiResponse<object>.Success(data));
    }
}

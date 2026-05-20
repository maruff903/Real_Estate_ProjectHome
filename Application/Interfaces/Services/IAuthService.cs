using RealEstateHub.Application.DTOs.Auth;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto, UserRole role, CancellationToken cancellationToken = default);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> LogoutAsync(RefreshTokenDto dto, CancellationToken cancellationToken = default);
}

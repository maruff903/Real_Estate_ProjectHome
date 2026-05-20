namespace RealEstateHub.Application.DTOs.Auth;

public record AuthResponseDto(
    string UserId,
    string FullName,
    string Email,
    string Role,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt);

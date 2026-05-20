using System.ComponentModel.DataAnnotations;

namespace RealEstateHub.Application.DTOs.Auth;

public record RefreshTokenDto([property: Required] string RefreshToken);

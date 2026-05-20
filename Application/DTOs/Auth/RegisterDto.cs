using System.ComponentModel.DataAnnotations;

namespace RealEstateHub.Application.DTOs.Auth;

public record RegisterDto(
    [property: Required]
    [property: StringLength(150, MinimumLength = 2)]
    string FullName,
    [property: Required]
    [property: EmailAddress]
    string Email,
    [property: Required]
    [property: Phone]
    string PhoneNumber,
    [property: Required]
    [property: MinLength(8)]
    string Password);

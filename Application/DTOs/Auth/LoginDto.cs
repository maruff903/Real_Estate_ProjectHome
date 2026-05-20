using System.ComponentModel.DataAnnotations;

namespace RealEstateHub.Application.DTOs.Auth;

public record LoginDto(
    [property: Required]
    [property: EmailAddress]
    string Email,
    [property: Required]
    string Password);

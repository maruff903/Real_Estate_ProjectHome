namespace RealEstateHub.Application.DTOs.Admin;

public record UserManagementDto(string Id, string FullName, string Email, string Role, bool IsBlocked, DateTime CreatedAt);

namespace RealEstateHub.Application.DTOs.Admin;


public class UserManagementDto
{
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsBlocked { get; set; }

    public DateTime CreatedAt { get; set; }
}

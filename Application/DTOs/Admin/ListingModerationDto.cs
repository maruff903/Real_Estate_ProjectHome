using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Admin;


public class ListingModerationDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string SellerId { get; set; } = string.Empty;

    public ListingStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}

using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.ContactRequests;


public class ContactRequestDto
{
    public int Id { get; set; }

    public int PropertyListingId { get; set; }

    public string ListingTitle { get; set; } = string.Empty;

    public string BuyerId { get; set; } = string.Empty;

    public string SellerId { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public ContactRequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}

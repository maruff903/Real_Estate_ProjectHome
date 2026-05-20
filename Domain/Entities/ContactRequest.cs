using RealEstateHub.Domain.Common;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Domain.Entities;

public class ContactRequest : BaseEntity
{
    public string BuyerId { get; set; } = string.Empty;
    public string SellerId { get; set; } = string.Empty;
    public Guid PropertyListingId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public ContactRequestStatus Status { get; set; } = ContactRequestStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PropertyListing? PropertyListing { get; set; }
}

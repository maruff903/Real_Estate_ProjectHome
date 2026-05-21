using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class Favorite : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int PropertyListingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PropertyListing? PropertyListing { get; set; }
}

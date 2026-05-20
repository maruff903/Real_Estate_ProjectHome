using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class PropertyImage : BaseEntity
{
    public Guid PropertyListingId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public PropertyListing? PropertyListing { get; set; }
}

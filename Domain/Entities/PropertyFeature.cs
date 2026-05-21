using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class PropertyFeature : BaseEntity
{
    public int PropertyListingId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Value { get; set; }

    public PropertyListing? PropertyListing { get; set; }
}

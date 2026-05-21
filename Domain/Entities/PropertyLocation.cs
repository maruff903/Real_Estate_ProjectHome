using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class PropertyLocation : BaseEntity
{
    public int PropertyListingId { get; set; }
    public string Address { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public PropertyListing? PropertyListing { get; set; }
}

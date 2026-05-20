using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class City : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<District> Districts { get; set; } = new List<District>();
    public ICollection<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
}

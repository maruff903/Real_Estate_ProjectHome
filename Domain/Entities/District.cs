using RealEstateHub.Domain.Common;

namespace RealEstateHub.Domain.Entities;

public class District : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public int CityId { get; set; }

    public City? City { get; set; }
    public ICollection<PropertyListing> Listings { get; set; } = new List<PropertyListing>();
}

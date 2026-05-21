using RealEstateHub.Domain.Common;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Domain.Entities;

public class PropertyListing : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PropertyType PropertyType { get; set; }
    public ListingType ListingType { get; set; }
    public decimal Price { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal Area { get; set; }
    public decimal? LandArea { get; set; }
    public int? RoomCount { get; set; }
    public int? BathroomCount { get; set; }
    public int? Floor { get; set; }
    public int? TotalFloors { get; set; }
    public string? BuildingBlock { get; set; }
    public bool HasGarage { get; set; }
    public bool HasGarden { get; set; }
    public bool HasBalcony { get; set; }
    public bool HasLift { get; set; }
    public bool HasFurniture { get; set; }
    public ListingStatus Status { get; set; } = ListingStatus.Pending;
    public string SellerId { get; set; } = string.Empty;
    public int CityId { get; set; }
    public int? DistrictId { get; set; }

    public City? City { get; set; }
    public District? District { get; set; }
    public PropertyLocation? Location { get; set; }
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public ICollection<PropertyFeature> Features { get; set; } = new List<PropertyFeature>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<ContactRequest> ContactRequests { get; set; } = new List<ContactRequest>();
}

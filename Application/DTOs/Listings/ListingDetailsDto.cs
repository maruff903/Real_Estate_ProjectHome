using RealEstateHub.Application.DTOs.Images;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Listings;

public class ListingDetailsDto
{
    public int Id { get; set; }
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
    public ListingStatus Status { get; set; }
    public string SellerId { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string? DistrictName { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public List<ImageDto> Images { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

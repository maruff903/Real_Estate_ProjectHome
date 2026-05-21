using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Listings;

public class ListingListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? MonthlyPrice { get; set; }
    public decimal Area { get; set; }
    public PropertyType PropertyType { get; set; }
    public ListingType ListingType { get; set; }
    public ListingStatus Status { get; set; }
    public string CityName { get; set; } = string.Empty;
    public string? DistrictName { get; set; }
    public string? MainImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

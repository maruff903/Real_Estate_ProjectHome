using RealEstateHub.Application.Pagination;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Listings;

public class ListingFilterDto : PaginationQuery
{
    public PropertyType? PropertyType { get; set; }
    public ListingType? ListingType { get; set; }
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinArea { get; set; }
    public decimal? MaxArea { get; set; }
    public int? RoomCount { get; set; }
    public int? Floor { get; set; }
    public string? BuildingBlock { get; set; }
    public bool? HasGarage { get; set; }
    public bool? HasGarden { get; set; }
    public bool? HasBalcony { get; set; }
    public bool? HasLift { get; set; }
    public bool? HasFurniture { get; set; }
    public ListingStatus? Status { get; set; }
    public string? SearchText { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}

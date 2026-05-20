using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Listings;

public record ListingListDto(
    Guid Id,
    string Title,
    decimal Price,
    decimal? MonthlyPrice,
    decimal Area,
    PropertyType PropertyType,
    ListingType ListingType,
    ListingStatus Status,
    string CityName,
    string? DistrictName,
    string? MainImageUrl,
    DateTime CreatedAt);

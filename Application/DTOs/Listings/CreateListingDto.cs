using System.ComponentModel.DataAnnotations;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Listings;

public class CreateListingDto : IValidatableObject
{
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(4000, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(PropertyType))]
    public PropertyType PropertyType { get; set; }

    [EnumDataType(typeof(ListingType))]
    public ListingType ListingType { get; set; }

    [Range(0.01, 999_999_999)]
    public decimal Price { get; set; }

    [Range(0.01, 999_999_999)]
    public decimal? MonthlyPrice { get; set; }

    [Range(1, 1_000_000)]
    public decimal Area { get; set; }

    [Range(1, 1_000_000)]
    public decimal? LandArea { get; set; }

    [Range(0, 100)]
    public int? RoomCount { get; set; }

    [Range(0, 100)]
    public int? BathroomCount { get; set; }

    [Range(-10, 300)]
    public int? Floor { get; set; }

    [Range(0, 300)]
    public int? TotalFloors { get; set; }

    [StringLength(50)]
    public string? BuildingBlock { get; set; }
    public bool HasGarage { get; set; }
    public bool HasGarden { get; set; }
    public bool HasBalcony { get; set; }
    public bool HasLift { get; set; }
    public bool HasFurniture { get; set; }

    [Required]
    public Guid CityId { get; set; }
    public Guid? DistrictId { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Address { get; set; } = string.Empty;

    [Range(-90, 90)]
    public decimal? Latitude { get; set; }

    [Range(-180, 180)]
    public decimal? Longitude { get; set; }

    [MaxLength(20)]
    public IReadOnlyList<string> ImageUrls { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ListingType == ListingType.Rent && MonthlyPrice is null)
        {
            yield return new ValidationResult("MonthlyPrice is required for rent listings.", [nameof(MonthlyPrice)]);
        }

        if (Floor.HasValue && TotalFloors.HasValue && Floor > TotalFloors)
        {
            yield return new ValidationResult("Floor cannot be greater than TotalFloors.", [nameof(Floor), nameof(TotalFloors)]);
        }

        foreach (var imageUrl in ImageUrls)
        {
            if (!Uri.TryCreate(imageUrl, UriKind.RelativeOrAbsolute, out var uri) ||
                (uri.IsAbsoluteUri && uri.Scheme is not ("http" or "https")) ||
                string.IsNullOrWhiteSpace(imageUrl))
            {
                yield return new ValidationResult("ImageUrls must contain valid relative, http, or https URLs.", [nameof(ImageUrls)]);
                yield break;
            }
        }
    }
}

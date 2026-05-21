using FluentValidation;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.Validators.Listings;

public abstract class ListingDtoValidator<T> : AbstractValidator<T>
    where T : CreateListingDto
{
    protected ListingDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(5, 200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(20, 4000);

        RuleFor(x => x.PropertyType)
            .IsInEnum();

        RuleFor(x => x.ListingType)
            .IsInEnum();

        RuleFor(x => x.Price)
            .InclusiveBetween(0.01m, 999_999_999m);

        RuleFor(x => x.MonthlyPrice)
            .InclusiveBetween(0.01m, 999_999_999m)
            .When(x => x.MonthlyPrice.HasValue);

        RuleFor(x => x.MonthlyPrice)
            .NotNull()
            .When(x => x.ListingType == ListingType.Rent)
            .WithMessage("MonthlyPrice is required for rent listings.");

        RuleFor(x => x.Area)
            .InclusiveBetween(1m, 1_000_000m);

        RuleFor(x => x.LandArea)
            .InclusiveBetween(1m, 1_000_000m)
            .When(x => x.LandArea.HasValue);

        RuleFor(x => x.RoomCount)
            .InclusiveBetween(0, 100)
            .When(x => x.RoomCount.HasValue);

        RuleFor(x => x.BathroomCount)
            .InclusiveBetween(0, 100)
            .When(x => x.BathroomCount.HasValue);

        RuleFor(x => x.Floor)
            .InclusiveBetween(-10, 300)
            .When(x => x.Floor.HasValue);

        RuleFor(x => x.TotalFloors)
            .InclusiveBetween(0, 300)
            .When(x => x.TotalFloors.HasValue);

        RuleFor(x => x)
            .Must(x => !x.Floor.HasValue || !x.TotalFloors.HasValue || x.Floor <= x.TotalFloors)
            .WithMessage("Floor cannot be greater than TotalFloors.");

        RuleFor(x => x.BuildingBlock)
            .MaximumLength(50);

        RuleFor(x => x.CityId)
            .GreaterThan(0);

        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .When(x => x.DistrictId.HasValue);

        RuleFor(x => x.Address)
            .NotEmpty()
            .Length(5, 500);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90m, 90m)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180m, 180m)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.ImageUrls)
            .NotNull()
            .Must(x => x.Count <= 20)
            .WithMessage("ImageUrls cannot contain more than 20 items.");

        RuleForEach(x => x.ImageUrls)
            .NotEmpty()
            .Must(BeValidImageUrl)
            .WithMessage("ImageUrls must contain valid relative, http, or https URLs.");
    }

    private static bool BeValidImageUrl(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return false;
        }

        return Uri.TryCreate(imageUrl, UriKind.RelativeOrAbsolute, out var uri) &&
               (!uri.IsAbsoluteUri || uri.Scheme is "http" or "https");
    }
}

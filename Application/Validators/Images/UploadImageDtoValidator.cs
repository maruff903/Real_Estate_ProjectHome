using FluentValidation;
using RealEstateHub.Application.DTOs.Images;

namespace RealEstateHub.Application.Validators.Images;

public class UploadImageDtoValidator : AbstractValidator<UploadImageDto>
{
    public UploadImageDtoValidator()
    {
        RuleFor(x => x.PropertyListingId)
            .GreaterThan(0);

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .Must(BeValidImageUrl)
            .WithMessage("ImageUrl must be a valid relative, http, or https URL.");
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

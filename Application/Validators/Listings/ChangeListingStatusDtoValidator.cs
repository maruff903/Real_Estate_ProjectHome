using FluentValidation;
using RealEstateHub.Application.DTOs.Listings;

namespace RealEstateHub.Application.Validators.Listings;

public class ChangeListingStatusDtoValidator : AbstractValidator<ChangeListingStatusDto>
{
    public ChangeListingStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();
    }
}

using FluentValidation;
using RealEstateHub.Application.DTOs.ContactRequests;

namespace RealEstateHub.Application.Validators.ContactRequests;

public class CreateContactRequestDtoValidator : AbstractValidator<CreateContactRequestDto>
{
    public CreateContactRequestDtoValidator()
    {
        RuleFor(x => x.PropertyListingId)
            .GreaterThan(0);

        RuleFor(x => x.Message)
            .NotEmpty()
            .Length(5, 2000);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(5, 30);
    }
}

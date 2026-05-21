using FluentValidation;
using RealEstateHub.Application.DTOs.Auth;

namespace RealEstateHub.Application.Validators.Auth;

public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .MaximumLength(1000);
    }
}

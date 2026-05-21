using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RealEstateHub.Application.Validators.Auth;

namespace RealEstateHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        return services;
    }
}

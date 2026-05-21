using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.WebAPI.Filters;

public class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var errors = new List<string>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (serviceProvider.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                errors.AddRange(result.Errors.Select(ToErrorMessage));
            }
        }

        if (errors.Count > 0)
        {
            context.Result = new BadRequestObjectResult(
                ApiResponse<object>.Failure("Validation failed.", StatusCodes.Status400BadRequest, errors));
            return;
        }

        await next();
    }

    private static string ToErrorMessage(ValidationFailure failure)
    {
        return string.IsNullOrWhiteSpace(failure.PropertyName)
            ? failure.ErrorMessage
            : $"{failure.PropertyName}: {failure.ErrorMessage}";
    }
}

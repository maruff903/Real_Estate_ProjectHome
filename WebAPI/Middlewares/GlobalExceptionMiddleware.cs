using System.Net;
using System.Text.Json;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.WebAPI.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<object>.Failure("Internal server error.", context.Response.StatusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

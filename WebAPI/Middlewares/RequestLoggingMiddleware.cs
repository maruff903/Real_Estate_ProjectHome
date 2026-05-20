namespace RealEstateHub.WebAPI.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("HTTP {Method} {Path}", context.Request.Method, context.Request.Path);
        await next(context);
    }
}

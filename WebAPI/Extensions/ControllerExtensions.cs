using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.WebAPI.Extensions;

public static class ControllerExtensions
{
    public static IActionResult FromResponse<T>(this ControllerBase controller, ApiResponse<T> response)
    {
        return controller.StatusCode(response.StatusCode, response);
    }

    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole("Admin");
    }
}

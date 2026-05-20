using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    protected bool IsAdmin => User.IsInRole("Admin");

    protected IActionResult FromResponse<T>(ApiResponse<T> response)
    {
        return StatusCode(response.StatusCode, response);
    }
}

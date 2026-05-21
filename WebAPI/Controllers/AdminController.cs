using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Pagination;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService, IContactRequestService contactRequestService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        return this.FromResponse(await adminService.GetDashboardStatsAsync(cancellationToken));
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        return this.FromResponse(await adminService.GetUsersAsync(query, cancellationToken));
    }

    [HttpPatch("users/{userId}/block")]
    public async Task<IActionResult> BlockUser(string userId, CancellationToken cancellationToken)
    {
        return this.FromResponse(await adminService.SetUserBlockedAsync(userId, true, cancellationToken));
    }

    [HttpPatch("users/{userId}/unblock")]
    public async Task<IActionResult> UnblockUser(string userId, CancellationToken cancellationToken)
    {
        return this.FromResponse(await adminService.SetUserBlockedAsync(userId, false, cancellationToken));
    }

    [HttpPatch("listings/{id:int}/moderate")]
    public async Task<IActionResult> ModerateListing(int id, ChangeListingStatusDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await adminService.ModerateListingAsync(id, dto, cancellationToken));
    }

    [HttpGet("contact-requests")]
    public async Task<IActionResult> ContactRequests(CancellationToken cancellationToken)
    {
        return this.FromResponse(await contactRequestService.GetAllAsync(cancellationToken));
    }
}

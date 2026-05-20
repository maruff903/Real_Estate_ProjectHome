using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Pagination;

namespace RealEstateHub.WebAPI.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService, IContactRequestService contactRequestService) : ApiControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        return FromResponse(await adminService.GetDashboardStatsAsync(cancellationToken));
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        return FromResponse(await adminService.GetUsersAsync(query, cancellationToken));
    }

    [HttpPatch("users/{userId}/block")]
    public async Task<IActionResult> BlockUser(string userId, CancellationToken cancellationToken)
    {
        return FromResponse(await adminService.SetUserBlockedAsync(userId, true, cancellationToken));
    }

    [HttpPatch("users/{userId}/unblock")]
    public async Task<IActionResult> UnblockUser(string userId, CancellationToken cancellationToken)
    {
        return FromResponse(await adminService.SetUserBlockedAsync(userId, false, cancellationToken));
    }

    [HttpPatch("listings/{id:guid}/moderate")]
    public async Task<IActionResult> ModerateListing(Guid id, ChangeListingStatusDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await adminService.ModerateListingAsync(id, dto, cancellationToken));
    }

    [HttpGet("contact-requests")]
    public async Task<IActionResult> ContactRequests(CancellationToken cancellationToken)
    {
        return FromResponse(await contactRequestService.GetAllAsync(cancellationToken));
    }
}

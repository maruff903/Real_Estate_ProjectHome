using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Seller,Admin")]
public class SellerListingsController(IListingService listingService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateListingDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.CreateAsync(dto, User.GetUserId(), cancellationToken));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateListingDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.UpdateAsync(id, dto, User.GetUserId(), User.IsAdmin(), cancellationToken));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(int id, ChangeListingStatusDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.ChangeStatusAsync(id, dto, User.GetUserId(), User.IsAdmin(), cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.DeleteAsync(id, User.GetUserId(), User.IsAdmin(), cancellationToken));
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

[Authorize(Roles = "Seller,Admin")]
public class SellerListingsController(IListingService listingService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateListingDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.CreateAsync(dto, UserId, cancellationToken));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateListingDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.UpdateAsync(id, dto, UserId, IsAdmin, cancellationToken));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeListingStatusDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.ChangeStatusAsync(id, dto, UserId, IsAdmin, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.DeleteAsync(id, UserId, IsAdmin, cancellationToken));
    }
}

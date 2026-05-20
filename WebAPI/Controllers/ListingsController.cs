using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

public class ListingsController(IListingService listingService) : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetListings([FromQuery] ListingFilterDto filter, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.GetListingsAsync(filter, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        return FromResponse(await listingService.GetByIdAsync(id, cancellationToken));
    }
}

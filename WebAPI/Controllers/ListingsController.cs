using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IListingService listingService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetListings([FromQuery] ListingFilterDto filter, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.GetListingsAsync(filter, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return this.FromResponse(await listingService.GetByIdAsync(id, cancellationToken));
    }
}

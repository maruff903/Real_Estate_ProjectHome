using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Buyer,Admin")]
public class FavoritesController(IFavoriteService favoriteService) : ControllerBase
{
    [HttpGet("mine")]
    public async Task<IActionResult> Mine(CancellationToken cancellationToken)
    {
        return this.FromResponse(await favoriteService.GetMineAsync(User.GetUserId(), cancellationToken));
    }

    [HttpPost("{listingId:int}")]
    public async Task<IActionResult> Add(int listingId, CancellationToken cancellationToken)
    {
        return this.FromResponse(await favoriteService.AddAsync(listingId, User.GetUserId(), cancellationToken));
    }

    [HttpDelete("{listingId:int}")]
    public async Task<IActionResult> Remove(int listingId, CancellationToken cancellationToken)
    {
        return this.FromResponse(await favoriteService.RemoveAsync(listingId, User.GetUserId(), cancellationToken));
    }
}

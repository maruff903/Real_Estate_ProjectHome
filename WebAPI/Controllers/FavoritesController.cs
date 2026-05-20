using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

[Authorize(Roles = "Buyer,Admin")]
public class FavoritesController(IFavoriteService favoriteService) : ApiControllerBase
{
    [HttpGet("mine")]
    public async Task<IActionResult> Mine(CancellationToken cancellationToken)
    {
        return FromResponse(await favoriteService.GetMineAsync(UserId, cancellationToken));
    }

    [HttpPost("{listingId:guid}")]
    public async Task<IActionResult> Add(Guid listingId, CancellationToken cancellationToken)
    {
        return FromResponse(await favoriteService.AddAsync(listingId, UserId, cancellationToken));
    }

    [HttpDelete("{listingId:guid}")]
    public async Task<IActionResult> Remove(Guid listingId, CancellationToken cancellationToken)
    {
        return FromResponse(await favoriteService.RemoveAsync(listingId, UserId, cancellationToken));
    }
}

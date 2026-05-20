using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.DTOs.Favorites;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Infrastructure.Services;

public class FavoriteService(IFavoriteRepository favorites, IListingRepository listings) : IFavoriteService
{
    public async Task<ApiResponse<object>> AddAsync(Guid listingId, string userId, CancellationToken cancellationToken = default)
    {
        if (await listings.GetByIdAsync(listingId, cancellationToken) is null)
        {
            return ApiResponse<object>.Failure("Listing not found.", 404);
        }

        if (await favorites.GetByUserAndListingAsync(userId, listingId, cancellationToken) is not null)
        {
            return ApiResponse<object>.Success(null, "Already in favorites.");
        }

        await favorites.AddAsync(new Favorite { UserId = userId, PropertyListingId = listingId }, cancellationToken);
        await favorites.SaveChangesAsync(cancellationToken);
        return ApiResponse<object>.Success(null, "Added to favorites.", 201);
    }

    public async Task<ApiResponse<object>> RemoveAsync(Guid listingId, string userId, CancellationToken cancellationToken = default)
    {
        var favorite = await favorites.GetByUserAndListingAsync(userId, listingId, cancellationToken);
        if (favorite is null)
        {
            return ApiResponse<object>.Failure("Favorite not found.", 404);
        }

        favorites.Delete(favorite);
        await favorites.SaveChangesAsync(cancellationToken);
        return ApiResponse<object>.Success(null, "Removed from favorites.");
    }

    public async Task<ApiResponse<IReadOnlyList<FavoriteDto>>> GetMineAsync(string userId, CancellationToken cancellationToken = default)
    {
        var favoritesList = await favorites.Query()
            .AsNoTracking()
            .Include(x => x.PropertyListing)
            .ThenInclude(x => x!.Images)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        var items = favoritesList.Select(x => new FavoriteDto(
                x.Id,
                x.PropertyListingId,
                x.PropertyListing!.Title,
                x.PropertyListing.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl ?? x.PropertyListing.Images.FirstOrDefault()?.ImageUrl,
                x.CreatedAt))
            .ToList();

        return ApiResponse<IReadOnlyList<FavoriteDto>>.Success(items);
    }
}

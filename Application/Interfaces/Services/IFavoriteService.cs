using RealEstateHub.Application.DTOs.Favorites;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<ApiResponse<object>> AddAsync(int listingId, string userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> RemoveAsync(int listingId, string userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<FavoriteDto>>> GetMineAsync(string userId, CancellationToken cancellationToken = default);
}

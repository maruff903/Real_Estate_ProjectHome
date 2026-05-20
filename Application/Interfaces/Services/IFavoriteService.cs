using RealEstateHub.Application.DTOs.Favorites;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IFavoriteService
{
    Task<ApiResponse<object>> AddAsync(Guid listingId, string userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> RemoveAsync(Guid listingId, string userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<FavoriteDto>>> GetMineAsync(string userId, CancellationToken cancellationToken = default);
}

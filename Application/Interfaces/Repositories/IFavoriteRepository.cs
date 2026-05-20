using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<Favorite?> GetByUserAndListingAsync(string userId, Guid listingId, CancellationToken cancellationToken = default);
}

using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IFavoriteRepository
{
    IQueryable<Favorite> Query();
    Task<Favorite?> GetByUserAndListingAsync(string userId, int listingId, CancellationToken cancellationToken = default);
    Task AddAsync(Favorite favorite, CancellationToken cancellationToken = default);
    void Delete(Favorite favorite);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

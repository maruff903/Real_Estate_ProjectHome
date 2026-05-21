using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class FavoriteRepository(AppDbContext dbContext) : IFavoriteRepository
{
    public IQueryable<Favorite> Query()
    {
        return dbContext.Favorites.AsQueryable();
    }

    public Task<Favorite?> GetByUserAndListingAsync(string userId, int listingId, CancellationToken cancellationToken = default)
    {
        return dbContext.Favorites.FirstOrDefaultAsync(x => x.UserId == userId && x.PropertyListingId == listingId, cancellationToken);
    }

    public Task AddAsync(Favorite favorite, CancellationToken cancellationToken = default)
    {
        return dbContext.Favorites.AddAsync(favorite, cancellationToken).AsTask();
    }

    public void Delete(Favorite favorite)
    {
        dbContext.Favorites.Remove(favorite);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

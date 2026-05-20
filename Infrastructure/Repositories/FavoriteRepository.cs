using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class FavoriteRepository(AppDbContext dbContext) : GenericRepository<Favorite>(dbContext), IFavoriteRepository
{
    public Task<Favorite?> GetByUserAndListingAsync(string userId, Guid listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Favorites.FirstOrDefaultAsync(x => x.UserId == userId && x.PropertyListingId == listingId, cancellationToken);
    }
}

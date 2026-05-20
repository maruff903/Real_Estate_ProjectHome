using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class ListingRepository(AppDbContext dbContext) : GenericRepository<PropertyListing>(dbContext), IListingRepository
{
    public IQueryable<PropertyListing> WithDetails()
    {
        return DbContext.PropertyListings
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.Location)
            .Include(x => x.Images)
            .AsQueryable();
    }

    public Task<PropertyListing?> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return WithDetails().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

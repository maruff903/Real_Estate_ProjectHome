using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class ListingRepository(AppDbContext dbContext) : IListingRepository
{
    public IQueryable<PropertyListing> Query()
    {
        return dbContext.PropertyListings.AsQueryable();
    }

    public Task<PropertyListing?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.PropertyListings.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public IQueryable<PropertyListing> WithDetails()
    {
        return dbContext.PropertyListings
            .Include(x => x.City)
            .Include(x => x.District)
            .Include(x => x.Location)
            .Include(x => x.Images)
            .AsQueryable();
    }

    public Task<PropertyListing?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return WithDetails().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task AddAsync(PropertyListing listing, CancellationToken cancellationToken = default)
    {
        return dbContext.PropertyListings.AddAsync(listing, cancellationToken).AsTask();
    }

    public void Update(PropertyListing listing)
    {
        dbContext.PropertyListings.Update(listing);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

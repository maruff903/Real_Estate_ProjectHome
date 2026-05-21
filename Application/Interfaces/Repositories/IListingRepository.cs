using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IListingRepository
{
    IQueryable<PropertyListing> Query();
    Task<PropertyListing?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    IQueryable<PropertyListing> WithDetails();
    Task<PropertyListing?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(PropertyListing listing, CancellationToken cancellationToken = default);
    void Update(PropertyListing listing);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

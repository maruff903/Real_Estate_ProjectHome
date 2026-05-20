using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IListingRepository : IGenericRepository<PropertyListing>
{
    IQueryable<PropertyListing> WithDetails();
    Task<PropertyListing?> GetDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}

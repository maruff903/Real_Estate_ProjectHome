using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IContactRequestRepository
{
    IQueryable<ContactRequest> Query();
    Task AddAsync(ContactRequest request, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

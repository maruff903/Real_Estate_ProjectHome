using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class ContactRequestRepository(AppDbContext dbContext) : IContactRequestRepository
{
    public IQueryable<ContactRequest> Query()
    {
        return dbContext.ContactRequests.AsQueryable();
    }

    public Task AddAsync(ContactRequest request, CancellationToken cancellationToken = default)
    {
        return dbContext.ContactRequests.AddAsync(request, cancellationToken).AsTask();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}

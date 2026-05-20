using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Repositories;

public class ContactRequestRepository(AppDbContext dbContext) : GenericRepository<ContactRequest>(dbContext), IContactRequestRepository;

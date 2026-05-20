namespace RealEstateHub.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<int> CountUsersInRoleAsync(string role, CancellationToken cancellationToken = default);
}

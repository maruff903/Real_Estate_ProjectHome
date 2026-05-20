using Microsoft.AspNetCore.Identity;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Repositories;

public class UserRepository(UserManager<AppUser> userManager) : IUserRepository
{
    public async Task<int> CountUsersInRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        var users = await userManager.GetUsersInRoleAsync(role);
        return users.Count;
    }
}

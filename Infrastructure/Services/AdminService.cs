using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.DTOs.Admin;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Pagination;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Services;

public class AdminService(
    IListingRepository listings,
    IUserRepository users,
    UserManager<AppUser> userManager,
    ICacheService cache) : IAdminService
{
    public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync(CancellationToken cancellationToken = default)
    {
        var stats = await cache.GetOrCreateAsync("dashboard-stats", async () =>
        {
            var query = listings.Query().AsNoTracking();
            return new DashboardStatsDto(
                await query.CountAsync(cancellationToken),
                await query.CountAsync(x => x.Status == ListingStatus.Sold, cancellationToken),
                await query.CountAsync(x => x.Status == ListingStatus.Rented, cancellationToken),
                await users.CountUsersInRoleAsync(UserRole.Seller.ToString(), cancellationToken),
                await users.CountUsersInRoleAsync(UserRole.Buyer.ToString(), cancellationToken),
                await query.CountAsync(x => x.Status == ListingStatus.Pending, cancellationToken),
                await query.CountAsync(x => x.Status == ListingStatus.Available || x.Status == ListingStatus.Approved, cancellationToken));
        }, TimeSpan.FromMinutes(2));

        return ApiResponse<DashboardStatsDto>.Success(stats);
    }

    public async Task<ApiResponse<PagedResponse<UserManagementDto>>> GetUsersAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var total = await userManager.Users.CountAsync(cancellationToken);
        var usersPage = await userManager.Users
            .OrderByDescending(x => x.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var items = new List<UserManagementDto>();
        foreach (var user in usersPage)
        {
            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty;
            items.Add(new UserManagementDto(user.Id, user.FullName, user.Email ?? string.Empty, role, user.IsBlocked, user.CreatedAt));
        }

        return ApiResponse<PagedResponse<UserManagementDto>>.Success(new PagedResponse<UserManagementDto>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = total
        });
    }

    public async Task<ApiResponse<object>> SetUserBlockedAsync(string userId, bool isBlocked, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return ApiResponse<object>.Failure("User not found.", 404);
        }

        user.IsBlocked = isBlocked;
        await userManager.UpdateAsync(user);
        return ApiResponse<object>.Success(null, isBlocked ? "User blocked." : "User unblocked.");
    }

    public async Task<ApiResponse<object>> ModerateListingAsync(Guid id, ChangeListingStatusDto dto, CancellationToken cancellationToken = default)
    {
        var listing = await listings.GetByIdAsync(id, cancellationToken);
        if (listing is null)
        {
            return ApiResponse<object>.Failure("Listing not found.", 404);
        }

        listing.Status = dto.Status;
        listings.Update(listing);
        await listings.SaveChangesAsync(cancellationToken);
        cache.Remove("dashboard-stats");
        cache.Remove($"listing:{id}");

        return ApiResponse<object>.Success(null, "Listing moderated.");
    }
}

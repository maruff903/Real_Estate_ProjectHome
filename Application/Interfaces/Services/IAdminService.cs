using RealEstateHub.Application.DTOs.Admin;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Pagination;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IAdminService
{
    Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<UserManagementDto>>> GetUsersAsync(PaginationQuery query, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> SetUserBlockedAsync(string userId, bool isBlocked, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> ModerateListingAsync(Guid id, ChangeListingStatusDto dto, CancellationToken cancellationToken = default);
}

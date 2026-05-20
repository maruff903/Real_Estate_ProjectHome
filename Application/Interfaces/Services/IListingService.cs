using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Pagination;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IListingService
{
    Task<ApiResponse<PagedResponse<ListingListDto>>> GetListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> CreateAsync(CreateListingDto dto, string sellerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> UpdateAsync(Guid id, UpdateListingDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> DeleteAsync(Guid id, string userId, bool isAdmin, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> ChangeStatusAsync(Guid id, ChangeListingStatusDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default);
}

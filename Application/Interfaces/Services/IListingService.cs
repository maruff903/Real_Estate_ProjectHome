using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Pagination;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IListingService
{
    Task<ApiResponse<PagedResponse<ListingListDto>>> GetListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> CreateAsync(CreateListingDto dto, string sellerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ListingDetailsDto>> UpdateAsync(int id, UpdateListingDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> DeleteAsync(int id, string userId, bool isAdmin, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> ChangeStatusAsync(int id, ChangeListingStatusDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default);
}

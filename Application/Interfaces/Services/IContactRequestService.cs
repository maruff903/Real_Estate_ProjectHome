using RealEstateHub.Application.DTOs.ContactRequests;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IContactRequestService
{
    Task<ApiResponse<ContactRequestDto>> CreateAsync(CreateContactRequestDto dto, string buyerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ContactRequestDto>>> GetForSellerAsync(string sellerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ContactRequestDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}

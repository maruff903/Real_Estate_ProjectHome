using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.DTOs.ContactRequests;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Entities;

namespace RealEstateHub.Infrastructure.Services;

public class ContactRequestService(IContactRequestRepository requests, IListingRepository listings) : IContactRequestService
{
    public async Task<ApiResponse<ContactRequestDto>> CreateAsync(CreateContactRequestDto dto, string buyerId, CancellationToken cancellationToken = default)
    {
        var listing = await listings.GetByIdAsync(dto.PropertyListingId, cancellationToken);
        if (listing is null)
        {
            return ApiResponse<ContactRequestDto>.Failure("Listing not found.", 404);
        }

        var request = new ContactRequest
        {
            BuyerId = buyerId,
            SellerId = listing.SellerId,
            PropertyListingId = listing.Id,
            Message = dto.Message,
            PhoneNumber = dto.PhoneNumber
        };

        await requests.AddAsync(request, cancellationToken);
        await requests.SaveChangesAsync(cancellationToken);

        return ApiResponse<ContactRequestDto>.Success(ToDto(request, listing.Title), "Request sent.", 201);
    }

    public async Task<ApiResponse<List<ContactRequestDto>>> GetForSellerAsync(string sellerId, CancellationToken cancellationToken = default)
    {
        var items = await requests.Query()
            .AsNoTracking()
            .Include(x => x.PropertyListing)
            .Where(x => x.SellerId == sellerId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => ToDto(x, x.PropertyListing!.Title))
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ContactRequestDto>>.Success(items);
    }

    public async Task<ApiResponse<List<ContactRequestDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await requests.Query()
            .AsNoTracking()
            .Include(x => x.PropertyListing)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => ToDto(x, x.PropertyListing!.Title))
            .ToListAsync(cancellationToken);

        return ApiResponse<List<ContactRequestDto>>.Success(items);
    }

    private static ContactRequestDto ToDto(ContactRequest request, string listingTitle)
    {
        return new ContactRequestDto
        {
            Id = request.Id,
            PropertyListingId = request.PropertyListingId,
            ListingTitle = listingTitle,
            BuyerId = request.BuyerId,
            SellerId = request.SellerId,
            Message = request.Message,
            PhoneNumber = request.PhoneNumber,
            Status = request.Status,
            CreatedAt = request.CreatedAt
        };
    }
}

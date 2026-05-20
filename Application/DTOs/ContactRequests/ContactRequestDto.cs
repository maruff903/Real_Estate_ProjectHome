using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.ContactRequests;

public record ContactRequestDto(
    Guid Id,
    Guid PropertyListingId,
    string ListingTitle,
    string BuyerId,
    string SellerId,
    string Message,
    string PhoneNumber,
    ContactRequestStatus Status,
    DateTime CreatedAt);

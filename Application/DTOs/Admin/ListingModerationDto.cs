using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Application.DTOs.Admin;

public record ListingModerationDto(Guid Id, string Title, string SellerId, ListingStatus Status, DateTime CreatedAt);

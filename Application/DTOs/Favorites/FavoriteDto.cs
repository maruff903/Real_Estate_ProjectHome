namespace RealEstateHub.Application.DTOs.Favorites;

public record FavoriteDto(Guid Id, Guid PropertyListingId, string Title, string? MainImageUrl, DateTime CreatedAt);

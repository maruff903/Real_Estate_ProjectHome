namespace RealEstateHub.Application.DTOs.Images;

public record UploadImageDto(Guid PropertyListingId, string ImageUrl, bool IsMain);

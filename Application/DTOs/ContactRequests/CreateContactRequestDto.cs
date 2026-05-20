namespace RealEstateHub.Application.DTOs.ContactRequests;

public record CreateContactRequestDto(Guid PropertyListingId, string Message, string PhoneNumber);

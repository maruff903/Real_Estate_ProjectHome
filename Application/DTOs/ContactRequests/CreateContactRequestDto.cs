namespace RealEstateHub.Application.DTOs.ContactRequests;


public class CreateContactRequestDto
{
    public int PropertyListingId { get; set; }

    public string Message { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}

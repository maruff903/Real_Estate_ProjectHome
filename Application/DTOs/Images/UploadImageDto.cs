namespace RealEstateHub.Application.DTOs.Images;

public class UploadImageDto
{
    public int PropertyListingId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsMain { get; set; }
}

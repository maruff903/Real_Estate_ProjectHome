namespace RealEstateHub.Application.DTOs.Favorites;

public class FavoriteDto
{
    public int Id { get; set; }
    public int PropertyListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? MainImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

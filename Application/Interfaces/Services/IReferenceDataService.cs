using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IReferenceDataService
{
    Task<ApiResponse<List<ReferenceItemDto>>> GetCitiesAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ReferenceItemDto>>> GetDistrictsAsync(int? cityId, CancellationToken cancellationToken = default);
}

public class ReferenceItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

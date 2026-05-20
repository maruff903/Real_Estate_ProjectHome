using RealEstateHub.Application.Responses;

namespace RealEstateHub.Application.Interfaces.Services;

public interface IReferenceDataService
{
    Task<ApiResponse<IReadOnlyList<ReferenceItemDto>>> GetCitiesAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<ReferenceItemDto>>> GetDistrictsAsync(Guid? cityId, CancellationToken cancellationToken = default);
}

public record ReferenceItemDto(Guid Id, string Name);

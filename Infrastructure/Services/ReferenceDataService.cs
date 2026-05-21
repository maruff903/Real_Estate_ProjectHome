using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.Services;

public class ReferenceDataService(AppDbContext dbContext, ICacheService cache) : IReferenceDataService
{
    public async Task<ApiResponse<List<ReferenceItemDto>>> GetCitiesAsync(CancellationToken cancellationToken = default)
    {
        var items = await cache.GetOrCreateAsync("cities", async () =>
            await dbContext.Cities
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceItemDto { Id = x.Id, Name = x.Name })
                .ToListAsync(cancellationToken));

        return ApiResponse<List<ReferenceItemDto>>.Success(items);
    }

    public async Task<ApiResponse<List<ReferenceItemDto>>> GetDistrictsAsync(int? cityId, CancellationToken cancellationToken = default)
    {
        var key = cityId.HasValue ? $"districts:{cityId}" : "districts:all";
        var items = await cache.GetOrCreateAsync(key, async () =>
        {
            var query = dbContext.Districts.AsNoTracking();
            if (cityId.HasValue)
            {
                query = query.Where(x => x.CityId == cityId);
            }

            return await query
                .OrderBy(x => x.Name)
                .Select(x => new ReferenceItemDto { Id = x.Id, Name = x.Name })
                .ToListAsync(cancellationToken);
        });

        return ApiResponse<List<ReferenceItemDto>>.Success(items);
    }
}

using Microsoft.EntityFrameworkCore;
using RealEstateHub.Application.DTOs.Images;
using RealEstateHub.Application.DTOs.Listings;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Pagination;
using RealEstateHub.Application.Responses;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;

namespace RealEstateHub.Infrastructure.Services;

public class ListingService(IListingRepository listings, ICacheService cache) : IListingService
{
    public async Task<ApiResponse<PagedResponse<ListingListDto>>> GetListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(listings.WithDetails().AsNoTracking(), filter);
        query = ApplySort(query, filter);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => ToListDto(x))
            .ToListAsync(cancellationToken);

        return ApiResponse<PagedResponse<ListingListDto>>.Success(new PagedResponse<ListingListDto>
        {
            Items = items,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = total
        });
    }

    public async Task<ApiResponse<ListingDetailsDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await cache.GetOrCreateAsync($"listing:{id}", () => listings.GetDetailsAsync(id, cancellationToken), TimeSpan.FromMinutes(5));
        if (listing is not null && !IsPublicStatus(listing.Status))
        {
            return ApiResponse<ListingDetailsDto>.Failure("Listing not found.", 404);
        }

        return listing is null
            ? ApiResponse<ListingDetailsDto>.Failure("Listing not found.", 404)
            : ApiResponse<ListingDetailsDto>.Success(ToDetailsDto(listing));
    }

    public async Task<ApiResponse<ListingDetailsDto>> CreateAsync(CreateListingDto dto, string sellerId, CancellationToken cancellationToken = default)
    {
        var listing = new PropertyListing();
        ApplyDto(listing, dto);
        listing.SellerId = sellerId;
        listing.Status = ListingStatus.Pending;

        await listings.AddAsync(listing, cancellationToken);
        await listings.SaveChangesAsync(cancellationToken);

        var created = await listings.GetDetailsAsync(listing.Id, cancellationToken);
        return ApiResponse<ListingDetailsDto>.Success(ToDetailsDto(created!), "Listing created and waiting for moderation.", 201);
    }

    public async Task<ApiResponse<ListingDetailsDto>> UpdateAsync(Guid id, UpdateListingDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var listing = await listings.GetDetailsAsync(id, cancellationToken);
        if (listing is null)
        {
            return ApiResponse<ListingDetailsDto>.Failure("Listing not found.", 404);
        }

        if (!isAdmin && listing.SellerId != userId)
        {
            return ApiResponse<ListingDetailsDto>.Failure("You do not own this listing.", 403);
        }

        ApplyDto(listing, dto);
        listings.Update(listing);
        await listings.SaveChangesAsync(cancellationToken);
        cache.Remove($"listing:{id}");

        return ApiResponse<ListingDetailsDto>.Success(ToDetailsDto(listing), "Listing updated.");
    }

    public async Task<ApiResponse<object>> DeleteAsync(Guid id, string userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var listing = await listings.GetByIdAsync(id, cancellationToken);
        if (listing is null)
        {
            return ApiResponse<object>.Failure("Listing not found.", 404);
        }

        if (!isAdmin && listing.SellerId != userId)
        {
            return ApiResponse<object>.Failure("You do not own this listing.", 403);
        }

        listing.IsDeleted = true;
        listing.Status = ListingStatus.Deleted;
        listings.Update(listing);
        await listings.SaveChangesAsync(cancellationToken);
        cache.Remove($"listing:{id}");

        return ApiResponse<object>.Success(null, "Listing deleted.");
    }

    public async Task<ApiResponse<object>> ChangeStatusAsync(Guid id, ChangeListingStatusDto dto, string userId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var listing = await listings.GetByIdAsync(id, cancellationToken);
        if (listing is null)
        {
            return ApiResponse<object>.Failure("Listing not found.", 404);
        }

        if (!isAdmin && listing.SellerId != userId)
        {
            return ApiResponse<object>.Failure("You do not own this listing.", 403);
        }

        if (!isAdmin && !CanSellerSetStatus(listing.Status, dto.Status))
        {
            return ApiResponse<object>.Failure("Seller cannot set this listing status.", 403);
        }

        listing.Status = dto.Status;
        listings.Update(listing);
        await listings.SaveChangesAsync(cancellationToken);
        cache.Remove($"listing:{id}");

        return ApiResponse<object>.Success(null, "Status changed.");
    }

    private static IQueryable<PropertyListing> ApplyFilter(IQueryable<PropertyListing> query, ListingFilterDto filter)
    {
        query = filter.Status switch
        {
            null => query.Where(x => x.Status == ListingStatus.Approved || x.Status == ListingStatus.Available),
            ListingStatus.Approved or ListingStatus.Available => query.Where(x => x.Status == filter.Status),
            _ => query.Where(x => false)
        };

        if (filter.PropertyType.HasValue) query = query.Where(x => x.PropertyType == filter.PropertyType);
        if (filter.ListingType.HasValue) query = query.Where(x => x.ListingType == filter.ListingType);
        if (filter.CityId.HasValue) query = query.Where(x => x.CityId == filter.CityId);
        if (filter.DistrictId.HasValue) query = query.Where(x => x.DistrictId == filter.DistrictId);
        if (filter.MinPrice.HasValue) query = query.Where(x => x.Price >= filter.MinPrice);
        if (filter.MaxPrice.HasValue) query = query.Where(x => x.Price <= filter.MaxPrice);
        if (filter.MinArea.HasValue) query = query.Where(x => x.Area >= filter.MinArea);
        if (filter.MaxArea.HasValue) query = query.Where(x => x.Area <= filter.MaxArea);
        if (filter.RoomCount.HasValue) query = query.Where(x => x.RoomCount == filter.RoomCount);
        if (filter.Floor.HasValue) query = query.Where(x => x.Floor == filter.Floor);
        if (!string.IsNullOrWhiteSpace(filter.BuildingBlock)) query = query.Where(x => x.BuildingBlock == filter.BuildingBlock);
        if (filter.HasGarage.HasValue) query = query.Where(x => x.HasGarage == filter.HasGarage);
        if (filter.HasGarden.HasValue) query = query.Where(x => x.HasGarden == filter.HasGarden);
        if (filter.HasBalcony.HasValue) query = query.Where(x => x.HasBalcony == filter.HasBalcony);
        if (filter.HasLift.HasValue) query = query.Where(x => x.HasLift == filter.HasLift);
        if (filter.HasFurniture.HasValue) query = query.Where(x => x.HasFurniture == filter.HasFurniture);
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            query = query.Where(x => x.Title.Contains(filter.SearchText) || x.Description.Contains(filter.SearchText));
        }

        return query;
    }

    private static bool IsPublicStatus(ListingStatus status)
    {
        return status is ListingStatus.Approved or ListingStatus.Available;
    }

    private static bool CanSellerSetStatus(ListingStatus currentStatus, ListingStatus newStatus)
    {
        if (newStatus is ListingStatus.Sold or ListingStatus.Rented or ListingStatus.Inactive)
        {
            return currentStatus is ListingStatus.Approved or ListingStatus.Available or ListingStatus.Sold or ListingStatus.Rented or ListingStatus.Inactive;
        }

        if (newStatus == ListingStatus.Available)
        {
            return currentStatus is ListingStatus.Approved or ListingStatus.Available or ListingStatus.Sold or ListingStatus.Rented or ListingStatus.Inactive;
        }

        return false;
    }

    private static IQueryable<PropertyListing> ApplySort(IQueryable<PropertyListing> query, ListingFilterDto filter)
    {
        var desc = string.Equals(filter.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        return filter.SortBy?.ToLowerInvariant() switch
        {
            "price" => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
            "area" => desc ? query.OrderByDescending(x => x.Area) : query.OrderBy(x => x.Area),
            "createdat" => desc ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }

    private static void ApplyDto(PropertyListing listing, CreateListingDto dto)
    {
        listing.Title = dto.Title;
        listing.Description = dto.Description;
        listing.PropertyType = dto.PropertyType;
        listing.ListingType = dto.ListingType;
        listing.Price = dto.Price;
        listing.MonthlyPrice = dto.MonthlyPrice;
        listing.Area = dto.Area;
        listing.LandArea = dto.LandArea;
        listing.RoomCount = dto.RoomCount;
        listing.BathroomCount = dto.BathroomCount;
        listing.Floor = dto.Floor;
        listing.TotalFloors = dto.TotalFloors;
        listing.BuildingBlock = dto.BuildingBlock;
        listing.HasGarage = dto.HasGarage;
        listing.HasGarden = dto.HasGarden;
        listing.HasBalcony = dto.HasBalcony;
        listing.HasLift = dto.HasLift;
        listing.HasFurniture = dto.HasFurniture;
        listing.CityId = dto.CityId;
        listing.DistrictId = dto.DistrictId;
        listing.Location = new PropertyLocation { PropertyListingId = listing.Id, Address = dto.Address, Latitude = dto.Latitude, Longitude = dto.Longitude };
        listing.Images = dto.ImageUrls.Select((url, index) => new PropertyImage { PropertyListingId = listing.Id, ImageUrl = url, IsMain = index == 0 }).ToList();
    }

    private static ListingListDto ToListDto(PropertyListing listing)
    {
        return new ListingListDto(
            listing.Id,
            listing.Title,
            listing.Price,
            listing.MonthlyPrice,
            listing.Area,
            listing.PropertyType,
            listing.ListingType,
            listing.Status,
            listing.City?.Name ?? string.Empty,
            listing.District?.Name,
            listing.Images.FirstOrDefault(x => x.IsMain)?.ImageUrl ?? listing.Images.FirstOrDefault()?.ImageUrl,
            listing.CreatedAt);
    }

    private static ListingDetailsDto ToDetailsDto(PropertyListing listing)
    {
        return new ListingDetailsDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            PropertyType = listing.PropertyType,
            ListingType = listing.ListingType,
            Price = listing.Price,
            MonthlyPrice = listing.MonthlyPrice,
            Area = listing.Area,
            LandArea = listing.LandArea,
            RoomCount = listing.RoomCount,
            BathroomCount = listing.BathroomCount,
            Floor = listing.Floor,
            TotalFloors = listing.TotalFloors,
            BuildingBlock = listing.BuildingBlock,
            HasGarage = listing.HasGarage,
            HasGarden = listing.HasGarden,
            HasBalcony = listing.HasBalcony,
            HasLift = listing.HasLift,
            HasFurniture = listing.HasFurniture,
            Status = listing.Status,
            SellerId = listing.SellerId,
            CityName = listing.City?.Name ?? string.Empty,
            DistrictName = listing.District?.Name,
            Address = listing.Location?.Address,
            Latitude = listing.Location?.Latitude,
            Longitude = listing.Location?.Longitude,
            Images = listing.Images.Select(x => new ImageDto(x.Id, x.ImageUrl, x.IsMain)).ToList(),
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt
        };
    }
}

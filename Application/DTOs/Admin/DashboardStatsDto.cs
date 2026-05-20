namespace RealEstateHub.Application.DTOs.Admin;

public record DashboardStatsDto(
    int TotalListings,
    int SoldListings,
    int RentedListings,
    int SellerCount,
    int BuyerCount,
    int PendingListings,
    int ActiveListings);

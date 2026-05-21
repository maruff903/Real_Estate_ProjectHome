namespace RealEstateHub.Application.DTOs.Admin;


public class DashboardStatsDto
{
    public int TotalListings { get; set; }

    public int SoldListings { get; set; }

    public int RentedListings { get; set; }

    public int SellerCount { get; set; }

    public int BuyerCount { get; set; }

    public int PendingListings { get; set; }

    public int ActiveListings { get; set; }
}

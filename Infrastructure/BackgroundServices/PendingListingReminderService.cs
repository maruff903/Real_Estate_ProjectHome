using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.BackgroundServices;

public class PendingListingReminderService(IServiceScopeFactory scopeFactory, ILogger<PendingListingReminderService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var pendingCount = await dbContext.PropertyListings.CountAsync(x => x.Status == ListingStatus.Pending, stoppingToken);
            dbContext.BackgroundJobLogs.Add(new BackgroundJobLog { JobName = nameof(PendingListingReminderService), Status = "Completed", Message = $"{pendingCount} listings are waiting for moderation." });
            await dbContext.SaveChangesAsync(stoppingToken);
            logger.LogInformation("{PendingCount} listings are waiting for moderation.", pendingCount);
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}

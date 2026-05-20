using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.BackgroundServices;

public class OldLogCleanupService(IServiceScopeFactory scopeFactory, ILogger<OldLogCleanupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var cutoff = DateTime.UtcNow.AddDays(-30);
            var oldLogs = await dbContext.BackgroundJobLogs.Where(x => x.StartedAt < cutoff).ToListAsync(stoppingToken);
            dbContext.BackgroundJobLogs.RemoveRange(oldLogs);
            dbContext.BackgroundJobLogs.Add(new BackgroundJobLog { JobName = nameof(OldLogCleanupService), Status = "Completed", Message = $"Removed {oldLogs.Count} old job logs." });
            await dbContext.SaveChangesAsync(stoppingToken);
            logger.LogInformation("Old log cleanup executed.");
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}

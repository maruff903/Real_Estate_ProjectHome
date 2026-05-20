using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Data;

namespace RealEstateHub.Infrastructure.BackgroundServices;

public class ExpiredListingCheckerService(IServiceScopeFactory scopeFactory, ILogger<ExpiredListingCheckerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.BackgroundJobLogs.Add(new BackgroundJobLog
        {
            JobName = nameof(ExpiredListingCheckerService),
            Status = "Completed",
            Message = "Expired listing check placeholder executed."
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Expired listing checker executed.");
    }
}

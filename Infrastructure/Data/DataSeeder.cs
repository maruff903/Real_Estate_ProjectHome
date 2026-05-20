using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Domain.Enums;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        if (!configuration.GetValue("Database:AutoMigrate", false))
        {
            return;
        }

        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DataSeeder));
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();

            foreach (var role in Enum.GetNames<UserRole>())
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await SeedAdminAsync(userManager, configuration);
            await SeedLocationsAsync(dbContext);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database migration/seed was skipped. Check ConnectionStrings:DefaultConnection and PostgreSQL credentials.");
        }
    }

    private static async Task SeedAdminAsync(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        var email = configuration["AdminSeed:Email"];
        var password = configuration["AdminSeed:Password"];
        var fullName = configuration["AdminSeed:FullName"] ?? "System Admin";

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new AppUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Admin seed failed: " + string.Join("; ", result.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
        {
            await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
        }
    }

    private static async Task SeedLocationsAsync(AppDbContext dbContext)
    {
        if (await dbContext.Cities.AnyAsync())
        {
            return;
        }

        var dushanbe = new City { Name = "Dushanbe" };
        var khujand = new City { Name = "Khujand" };
        dbContext.Cities.AddRange(dushanbe, khujand);
        dbContext.Districts.AddRange(
            new District { Name = "Ismoili Somoni", City = dushanbe },
            new District { Name = "Sino", City = dushanbe },
            new District { Name = "Firdavsi", City = dushanbe },
            new District { Name = "Markaz", City = khujand });

        await dbContext.SaveChangesAsync();
    }
}

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstateHub.Application.Interfaces.Repositories;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Infrastructure.BackgroundServices;
using RealEstateHub.Infrastructure.Caching;
using RealEstateHub.Infrastructure.Configurations;
using RealEstateHub.Infrastructure.Data;
using RealEstateHub.Infrastructure.Identity;
using RealEstateHub.Infrastructure.Repositories;
using RealEstateHub.Infrastructure.Services;

namespace RealEstateHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        var jwt = configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.AddAuthorization();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IContactRequestService, ContactRequestService>();
        services.AddScoped<IReferenceDataService, ReferenceDataService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IFileService, LocalFileService>();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        if (configuration.GetValue("BackgroundJobSettings:Enabled", false))
        {
            services.AddHostedService<ExpiredListingCheckerService>();
            services.AddHostedService<OldLogCleanupService>();
            services.AddHostedService<PendingListingReminderService>();
        }

        return services;
    }
}

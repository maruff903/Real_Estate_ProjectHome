using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateHub.Domain.Common;
using RealEstateHub.Domain.Entities;
using RealEstateHub.Infrastructure.Identity;

namespace RealEstateHub.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<PropertyListing> PropertyListings => Set<PropertyListing>();
    public DbSet<PropertyImage> PropertyImages => Set<PropertyImage>();
    public DbSet<PropertyLocation> PropertyLocations => Set<PropertyLocation>();
    public DbSet<PropertyFeature> PropertyFeatures => Set<PropertyFeature>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<BackgroundJobLog> BackgroundJobLogs => Set<BackgroundJobLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PropertyListing>(entity =>
        {
            entity.HasQueryFilter(x => !x.IsDeleted);
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            entity.Property(x => x.Price).HasPrecision(18, 2);
            entity.Property(x => x.MonthlyPrice).HasPrecision(18, 2);
            entity.Property(x => x.Area).HasPrecision(18, 2);
            entity.Property(x => x.LandArea).HasPrecision(18, 2);
            entity.Property(x => x.PropertyType).HasConversion<string>().HasMaxLength(40);
            entity.Property(x => x.ListingType).HasConversion<string>().HasMaxLength(40);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(40);
            entity.HasOne(x => x.City).WithMany(x => x.Listings).HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.District).WithMany(x => x.Listings).HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<PropertyLocation>(entity =>
        {
            entity.HasQueryFilter(x => x.PropertyListing != null && !x.PropertyListing.IsDeleted);
            entity.Property(x => x.Address).HasMaxLength(500).IsRequired();
            entity.Property(x => x.Latitude).HasPrecision(10, 7);
            entity.Property(x => x.Longitude).HasPrecision(10, 7);
            entity.HasOne(x => x.PropertyListing).WithOne(x => x.Location).HasForeignKey<PropertyLocation>(x => x.PropertyListingId);
        });

        builder.Entity<PropertyImage>(entity =>
        {
            entity.HasQueryFilter(x => x.PropertyListing != null && !x.PropertyListing.IsDeleted);
            entity.Property(x => x.ImageUrl).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.PropertyListing).WithMany(x => x.Images).HasForeignKey(x => x.PropertyListingId);
        });

        builder.Entity<PropertyFeature>(entity =>
        {
            entity.HasQueryFilter(x => x.PropertyListing != null && !x.PropertyListing.IsDeleted);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Value).HasMaxLength(500);
            entity.HasOne(x => x.PropertyListing).WithMany(x => x.Features).HasForeignKey(x => x.PropertyListingId);
        });

        builder.Entity<Favorite>(entity =>
        {
            entity.HasQueryFilter(x => x.PropertyListing != null && !x.PropertyListing.IsDeleted);
            entity.HasIndex(x => new { x.UserId, x.PropertyListingId }).IsUnique();
            entity.HasOne(x => x.PropertyListing).WithMany(x => x.Favorites).HasForeignKey(x => x.PropertyListingId);
        });

        builder.Entity<ContactRequest>(entity =>
        {
            entity.HasQueryFilter(x => x.PropertyListing != null && !x.PropertyListing.IsDeleted);
            entity.Property(x => x.Message).HasMaxLength(2000).IsRequired();
            entity.Property(x => x.PhoneNumber).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(40);
            entity.HasOne(x => x.PropertyListing).WithMany(x => x.ContactRequests).HasForeignKey(x => x.PropertyListingId);
        });

        builder.Entity<City>(entity =>
        {
            entity.HasQueryFilter(x => !x.IsDeleted);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });

        builder.Entity<District>(entity =>
        {
            entity.HasQueryFilter(x => !x.IsDeleted);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.HasOne(x => x.City).WithMany(x => x.Districts).HasForeignKey(x => x.CityId);
        });

        builder.Entity<Category>(entity =>
        {
            entity.HasQueryFilter(x => !x.IsDeleted);
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.Property(x => x.Token).HasMaxLength(256).IsRequired();
            entity.HasIndex(x => x.Token).IsUnique();
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

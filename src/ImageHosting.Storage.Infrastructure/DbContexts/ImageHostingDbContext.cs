using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Infrastructure.DbContexts;

public class ImageHostingDbContext(DbContextOptions<ImageHostingDbContext> options)
    : DbContext(options), IImageHostingDbContext
{
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ImageTag> ImageTags => Set<ImageTag>();
    public DbSet<ForbiddenCategory> ForbiddenCategories => Set<ForbiddenCategory>();

    public void Migrate() => Database.Migrate();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ForbiddenCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        modelBuilder.ApplyConfiguration(new ImageTagConfiguration());
    }
}

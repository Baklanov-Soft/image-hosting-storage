using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public class ImageHostingDbContext(DbContextOptions<ImageHostingDbContext> options)
    : DbContext(options), IImageHostingDbContext
{
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ForbiddenCategory> ForbiddenCategories => Set<ForbiddenCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageHostingDbContext).Assembly);
    }

    public void Migrate() => Database.Migrate();
}

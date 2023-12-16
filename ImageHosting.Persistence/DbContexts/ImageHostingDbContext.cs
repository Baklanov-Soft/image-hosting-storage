using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public class ImageHostingDbContext : DbContext, IImageHostingDbContext
{
    public ImageHostingDbContext(DbContextOptions<ImageHostingDbContext> options) : base(options)
    {
    }

    public DbSet<Image> Images => Set<Image>();
    public DbSet<ForbiddenCategory> ForbiddenCategories => Set<ForbiddenCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageHostingDbContext).Assembly);
    }
}

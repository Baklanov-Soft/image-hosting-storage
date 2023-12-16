using ImageHosting.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.DbContexts;

public class ImageHostingDbContext : DbContext, IImageHostingDbContext
{
    public ImageHostingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Image> Images => Set<Image>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ImageCategory> ImageCategories => Set<ImageCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageHostingDbContext).Assembly);
    }
}

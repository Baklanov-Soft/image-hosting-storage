using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public class ImageHostingDbContext(DbContextOptions<ImageHostingDbContext> options)
    : DbContext(options), IImageHostingDbContext
{
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ImageTag> ImageTags => Set<ImageTag>();
    public DbSet<ForbiddenCategory> ForbiddenCategories => Set<ForbiddenCategory>();

    public void Migrate() => Database.Migrate();
}

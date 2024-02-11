using ImageHosting.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public class ImageHostingDbContext(DbContextOptions<ImageHostingDbContext> options)
    : IdentityDbContext(options), IImageHostingDbContext
{
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ForbiddenCategory> ForbiddenCategories => Set<ForbiddenCategory>();

    public void Migrate() => Database.Migrate();
}

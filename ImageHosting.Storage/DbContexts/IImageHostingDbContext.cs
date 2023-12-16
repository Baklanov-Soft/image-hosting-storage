using ImageHosting.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.DbContexts;

public interface IImageHostingDbContext
{
    DbSet<Image> Images { get; }
    DbSet<Category> Categories { get; }
    DbSet<ImageCategory> ImageCategories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

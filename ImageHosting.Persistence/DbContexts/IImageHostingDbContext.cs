using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public interface IImageHostingDbContext
{
    DbSet<Image> Images { get; }
    DbSet<ForbiddenCategory> ForbiddenCategories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

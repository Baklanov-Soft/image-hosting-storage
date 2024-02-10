using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public interface IImageHostingDbContext
{
    DbSet<Image> Images { get; }
    DbSet<ForbiddenCategory> ForbiddenCategories { get; }
    
    /// <inheritdoc cref="DbContext"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void Migrate();
}

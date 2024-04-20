using ImageHosting.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.DbContexts;

public interface IImageHostingDbContext
{
    DbSet<Image> Images { get; }
    DbSet<ImageTag> ImageTags { get; }
    DbSet<ForbiddenCategory> ForbiddenCategories { get; }

    /// <inheritdoc cref="DbContext"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc cref="RelationalDatabaseFacadeExtensions.Migrate"/>
    void Migrate();
}
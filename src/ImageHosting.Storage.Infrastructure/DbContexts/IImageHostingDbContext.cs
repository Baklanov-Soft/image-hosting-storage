using ImageHosting.Storage.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Infrastructure.DbContexts;

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
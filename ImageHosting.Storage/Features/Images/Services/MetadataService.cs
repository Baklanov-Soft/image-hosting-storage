using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataService(IImageHostingDbContext dbContext) : IMetadataService
{
    public async Task<ReadImageResponse> WriteMetadataAsync(ImageMetadata imageMetadata,
        CancellationToken cancellationToken = default)
    {
        var entity = imageMetadata.ToEntity();
        dbContext.Images.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ReadImageResponse.From(entity);
    }

    public async Task<bool> DeleteMetadataAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rowsAffected = await dbContext.Images
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        return rowsAffected == 1;
    }
}

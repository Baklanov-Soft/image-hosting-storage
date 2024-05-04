using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataService(IImageHostingDbContext dbContext, ILogger<MetadataService> logger) : IMetadataService
{
    public async Task<ImageUploadedResponse> WriteMetadataAsync(ImageMetadata imageMetadata,
        CancellationToken cancellationToken = default)
    {
        var entity = imageMetadata.ToEntity();
        dbContext.Images.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogMetadataWritten(entity.Id);
        return ImageUploadedResponse.From(entity);
    }

    public async Task DeleteMetadataAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        await dbContext.Images
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        
        logger.LogMetadataDeleted(id);
    }
}

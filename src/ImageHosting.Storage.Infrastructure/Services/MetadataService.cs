using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Infrastructure.Services;

public class MetadataService(IImageHostingDbContext dbContext, ILogger<MetadataService> logger) : IMetadataService
{
    public async Task<ImageUploadedDto> WriteMetadataAsync(ImageMetadataDto imageMetadataDto,
        CancellationToken cancellationToken = default)
    {
        var entity = imageMetadataDto.ToEntity();
        dbContext.Images.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogMetadataWritten(entity.Id);
        return ImageUploadedDto.From(entity);
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

using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Infrastructure.Services;

public class MetadataService(IImageHostingDbContext dbContext) : IMetadataService
{
    public async Task<ImageUploadedDto> WriteMetadataAsync(ImageMetadataDto imageMetadataDto,
        CancellationToken cancellationToken = default)
    {
        var entity = imageMetadataDto.ToEntity();
        dbContext.Images.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return ImageUploadedDto.From(entity);
    }

    public async Task<bool> DeleteMetadataAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var rowsAffected = await dbContext.Images
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);
        return rowsAffected == 1;
    }
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Services;

public class ImageMetadataService(IImageHostingDbContext dbContext) : IImageMetadataService
{
    public async Task WriteMetadataAsync(ImageMetadata imageMetadata, CancellationToken cancellationToken = default)
    {
        var readImageDto = await dbContext.Images
            .Where(i => i.ObjectName == imageMetadata.ObjectName)
            .ToReadImageDtos()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);
        if (readImageDto is not null)
        {
            throw new ImageMetadataAlreadyExistsException(imageMetadata.ObjectName);
        }
        
        var entity = imageMetadata.ToImage();
        dbContext.Images.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

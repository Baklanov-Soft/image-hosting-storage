using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Extensions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IGetImageHandler
{
    Task<ReadImage> GetAsync(ImageId id, CancellationToken cancellationToken = default);
}

public class GetImageHandler(IImageHostingDbContext dbContext, IOptions<MinioOptions> minioOptions) : IGetImageHandler
{
    public async Task<ReadImage> GetAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var image = await dbContext.Images
            .Where(i => i.Id == id)
            .ToReadImages(minioOptions.Value.Endpoint)
            .FirstOrDefaultAsync(cancellationToken);

        if (image == default)
        {
            throw new ImageNotFoundException(id);
        }

        return image;
    }
}
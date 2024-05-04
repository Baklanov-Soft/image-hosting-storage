using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.WebApi.Features.Images.Exceptions;
using ImageHosting.Storage.WebApi.Features.Images.Extensions;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using ImageHosting.Storage.WebApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IGetImageHandler
{
    Task<ReadImage> GetAsync(ImageId id, CancellationToken cancellationToken = default);
}

public class GetImageHandler(IImageHostingDbContext dbContext, IOptions<ImagesOptions> options) : IGetImageHandler
{
    private readonly ImagesOptions _options = options.Value;

    public async Task<ReadImage> GetAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var image = await dbContext.Images
            .AsNoTracking()
            .Where(i => i.Id == id)
            .ToReadImages(_options.BaseUri)
            .FirstOrDefaultAsync(cancellationToken);
        if (image is null)
        {
            throw new ImageMetadataNotFoundException(id);
        }

        return image;
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Extensions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Handlers;

public interface IGetImageHandler
{
    Task<ReadImage?> GetAsync(ImageId id, CancellationToken cancellationToken = default);
}

public class GetImageHandler(IImageHostingDbContext dbContext, IOptions<ImagesOptions> options) : IGetImageHandler
{
    private readonly ImagesOptions _options = options.Value;

    public async Task<ReadImage?> GetAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        var image = await dbContext.Images
            .AsNoTracking()
            .Where(i => i.Id == id)
            .ToReadImages(_options.BaseUri)
            .FirstOrDefaultAsync(cancellationToken);

        return image;
    }
}
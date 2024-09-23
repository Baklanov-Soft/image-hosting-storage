using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.WebApi.Features.Images.Extensions;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using ImageHosting.Storage.WebApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IGetImageHandler
{
    Task<ReadImage?> GetAsync(ImageId id, CancellationToken cancellationToken = default);
}

public class GetImageHandler(IImageHostingDbContext dbContext, IOptions<ImagesOptions> options) : IGetImageHandler
{
    private readonly Uri _publicUrl = options.Value.PublicUrl;

    public Task<ReadImage?> GetAsync(ImageId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Images
            .AsNoTracking()
            .Where(i => i.Id == id)
            .ToReadImages(_publicUrl)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
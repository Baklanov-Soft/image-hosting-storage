using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface ISetHidenessHandler
{
    Task<ReadImage?> SetHidenessAsync(ImageId imageId, bool hidden, CancellationToken ct = default);
}

public class SetHidenessHandler(IImageHostingDbContext dbContext, IGetImageHandler getImageHandler) : ISetHidenessHandler
{
    public async Task<ReadImage?> SetHidenessAsync(ImageId imageId, bool hidden, CancellationToken ct = default)
    {
        await dbContext.Images
            .Where(i => i.Id == imageId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(i => i.Hidden, hidden), ct);

        var readImage = await getImageHandler.GetAsync(imageId, ct);
        return readImage;
    }
}

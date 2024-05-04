using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IUpdateNameHandler
{
    Task<ReadImage> UpdateAsync(ImageId imageId, string newName, CancellationToken cancellationToken = default);
}

public class UpdateNameHandler(IImageHostingDbContext dbContext, IGetImageHandler getImageHandler) : IUpdateNameHandler
{
    public async Task<ReadImage> UpdateAsync(ImageId imageId, string newName,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Images
            .Where(i => i.Id == imageId)
            .ExecuteUpdateAsync(
                calls => calls.SetProperty(i => i.ObjectName, newName),
                cancellationToken);

        var readImage = await getImageHandler.GetAsync(imageId, cancellationToken);
        return readImage;
    }
}
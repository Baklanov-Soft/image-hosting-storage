using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Handlers;

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
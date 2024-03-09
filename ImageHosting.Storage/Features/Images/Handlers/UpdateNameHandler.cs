using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Handlers;

public interface IUpdateNameHandler
{
    Task<ReadImage> UpdateAsync(ImageId imageId, string newName, CancellationToken cancellationToken = default);
}

public class UpdateNameHandler(IImageHostingDbContext dbContext, IOptions<ImagesOptions> options) : IUpdateNameHandler
{
    private readonly ImagesOptions _options = options.Value;

    public async Task<ReadImage> UpdateAsync(ImageId imageId, string newName,
        CancellationToken cancellationToken = default)
    {
        var imageEntity = await dbContext.Images
            .Where(i => i.Id == imageId)
            .FirstOrDefaultAsync(cancellationToken);
        if (imageEntity is null)
        {
            throw new ImageMetadataNotFoundException(imageId);
        }

        imageEntity.ObjectName = newName;
        await dbContext.SaveChangesAsync(cancellationToken);

        return ReadImage.From(imageEntity, _options.BaseUri);
    }
}
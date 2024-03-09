using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Handlers;

public interface IAddTagsHandler
{
    Task<AddTagsResponse> AddAsync(ImageId id, IEnumerable<string> tags,
        CancellationToken cancellationToken = default);
}

public class AddTagsHandler(IImageHostingDbContext dbContext) : IAddTagsHandler
{
    public async Task<AddTagsResponse> AddAsync(ImageId id, IEnumerable<string> tags,
        CancellationToken cancellationToken = default)
    {
        var image = await dbContext.Images
            .Include(i => i.Tags)
            .AsSplitQuery()
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        if (image is null)
        {
            throw new ImageMetadataNotFoundException(id);
        }

        image.AddTags(tags);

        await dbContext.SaveChangesAsync(cancellationToken);
        return new AddTagsResponse
        {
            Tags = image.Tags.Select(t => t.TagName).ToList()
        };
    }
}
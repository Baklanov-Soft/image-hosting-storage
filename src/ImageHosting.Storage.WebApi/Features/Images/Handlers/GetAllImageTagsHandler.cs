using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public class GetAllImageTagsHandler(IImageHostingDbContext dbContext) : IGetAllImageTagsHandler
{
    public Task<List<string>> GetTagsAsync(ImageId imageId, CancellationToken ct = default)
    {
        return dbContext.ImageTags
            .Where(it => it.ImageId == imageId)
            .Select(it => it.TagName)
            .ToListAsync(ct);
    }
}

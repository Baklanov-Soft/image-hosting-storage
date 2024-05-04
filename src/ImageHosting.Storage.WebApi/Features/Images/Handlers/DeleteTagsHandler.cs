using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IDeleteTagsHandler
{
    Task DeleteAsync(ImageId id, IReadOnlyList<string> tags, CancellationToken cancellationToken = default);
}

public class DeleteTagsHandler(IImageHostingDbContext dbContext) : IDeleteTagsHandler
{
    public Task DeleteAsync(ImageId id, IReadOnlyList<string> tags, CancellationToken cancellationToken = default)
    {
        return dbContext.ImageTags
            .Where(it => it.ImageId == id && tags.Contains(it.TagName))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
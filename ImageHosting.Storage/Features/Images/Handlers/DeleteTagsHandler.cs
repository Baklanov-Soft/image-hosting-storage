using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Handlers;

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
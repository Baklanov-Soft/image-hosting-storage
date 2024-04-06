using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.Features.Images.Services;

public class AssignTagsService(IImageHostingDbContext dbContext) : IAssignTagsService
{
    public async Task AssignTagsAsync(ImageId id, Dictionary<string, double> categories, double threshold,
        CancellationToken cancellationToken = default)
    {
        var image = await dbContext.Images
            .AsTracking()
            .Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        image?.AddTags(categories
            .Where(pair => pair.Value > threshold)
            .Select(pair => pair.Key));

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
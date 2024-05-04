using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Infrastructure.Services;

public class AssignTagsService(IImageHostingDbContext dbContext, IOptions<AssignTagsOptions> options)
    : IAssignTagsService
{
    private readonly double _threshold = options.Value.Threshold;

    public async Task AssignTagsAsync(Dictionary<ImageId, Dictionary<string, double>> categories,
        CancellationToken cancellationToken = default)
    {
        var images = await dbContext.Images
            .AsTracking()
            .Include(i => i.Tags)
            .Where(i => categories.Keys.Contains(i.Id))
            .ToListAsync(cancellationToken);

        foreach (var image in images)
        {
            image.AddTags(categories[image.Id]
                .Where(category => category.Value > _threshold)
                .Select(category => category.Key));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
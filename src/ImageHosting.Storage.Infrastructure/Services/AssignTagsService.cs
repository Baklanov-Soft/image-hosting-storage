using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Logging;
using ImageHosting.Storage.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Infrastructure.Services;

public class AssignTagsService(IImageHostingDbContext dbContext, IOptions<AssignTagsOptions> options, ILogger<AssignTagsService> logger)
    : IAssignTagsService
{
    private readonly double _threshold = options.Value.Threshold;
    private readonly LogTags _logTags = new(logger);

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
            var tags = categories[image.Id]
                .Where(category => category.Value > _threshold)
                .Select(category => category.Key)
                .ToArray();

            image.AddTags(tags);
            _logTags.Add(image.Id, tags);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        _logTags.Log();
    }

    private sealed class LogTags(ILogger<AssignTagsService> logger)
    {
        private readonly Dictionary<ImageId, IEnumerable<string>> _imageTags = [];

        public void Add(ImageId imageId, IEnumerable<string> tags)
        {
            _imageTags.Add(imageId, tags);
        }

        public void Log()
        {
            foreach (var (imageId, tags) in _imageTags)
            {
                logger.LogImageTagsAssigned(tags, imageId);
            }
        }
    }
}

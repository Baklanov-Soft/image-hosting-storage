using System.Linq;
using System.Threading.Tasks;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage;

public class AssignTagsConsumer(IOptions<KafkaOptions> kafkaOptions, IImageHostingDbContext dbContext)
    : IConsumer<Batch<CategorizedNewImage>>
{
    private readonly double _threshold = kafkaOptions.Value.CategoriesConsumer.Threshold;

    public async Task Consume(ConsumeContext<Batch<CategorizedNewImage>> context)
    {
        var messages = context.Message
            .GroupBy(consumeContext => consumeContext.Message.ImageId,
                consumeContext => consumeContext.Message.Categories)
            .ToDictionary(grouping => grouping.Key,
                grouping => grouping.SelectMany(categories => categories)
                    .ToDictionary(category => category.Key, category => category.Value));

        var images = await dbContext.Images
            .AsTracking()
            .Include(i => i.Tags)
            .Where(i => messages.Keys.Contains(i.Id))
            .ToListAsync(context.CancellationToken);

        foreach (var image in images)
        {
            image.AddTags(messages[image.Id]
                .Where(category => category.Value > _threshold)
                .Select(category => category.Key));
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
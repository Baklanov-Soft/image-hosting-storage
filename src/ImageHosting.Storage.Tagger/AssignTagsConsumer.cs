using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Messages;
using MassTransit;

namespace ImageHosting.Storage.Tagger;

public class AssignTagsConsumer(IAssignTagsService assignTagsService) : IConsumer<Batch<CategorizedNewImage>>
{
    public Task Consume(ConsumeContext<Batch<CategorizedNewImage>> context)
    {
        var messages = context.Message
            .GroupBy(consumeContext => consumeContext.Message.Image.ImageId,
                consumeContext => consumeContext.Message.Categories)
            .ToDictionary(grouping => grouping.Key,
                grouping => grouping.SelectMany(categories => categories)
                    .ToDictionary(category => category.Key, category => category.Value));

        return assignTagsService.AssignTagsAsync(messages, context.CancellationToken);
    }
}
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Infrastructure.Services;

public class NewImageProducer(ITopicProducer<NewImage> messageProducer, ILogger<NewImageProducer> logger)
    : INewImageProducer
{
    public async Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        await messageProducer.Produce(newImage, cancellationToken);
        logger.LogMessagePublished(newImage.ImageId, newImage.BucketId);
    }
}
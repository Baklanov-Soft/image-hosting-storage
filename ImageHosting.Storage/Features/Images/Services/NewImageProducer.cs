using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Logging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Features.Images.Services;

public class NewImageProducer(ITopicProducer<NewImage> messageProducer, ILogger<NewImageProducer> logger)
    : INewImageProducer
{
    public async Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        await messageProducer.Produce(newImage, cancellationToken);
        logger.LogMessagePublished(newImage.ImageId, newImage.BucketId);
    }
}
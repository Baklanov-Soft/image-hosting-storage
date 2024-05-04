using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Messages;
using MassTransit;

namespace ImageHosting.Storage.Infrastructure.Services;

public class NewImageProducer(ITopicProducer<NewImage> messageProducer) : INewImageProducer
{
    public Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        return messageProducer.Produce(newImage, cancellationToken);
    }
}
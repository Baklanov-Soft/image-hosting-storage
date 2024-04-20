using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using MassTransit;

namespace ImageHosting.Storage.Features.Images.Services;

public class NewImageProducer(ITopicProducer<NewImage> messageProducer) : INewImageProducer
{
    public Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        return messageProducer.Produce(newImage, cancellationToken);
    }
}
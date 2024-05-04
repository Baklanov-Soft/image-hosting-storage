using ImageHosting.Storage.Domain.Messages;

namespace ImageHosting.Storage.Application.Services;

public interface INewImageProducer
{
    Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default);
}
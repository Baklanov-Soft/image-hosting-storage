using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IPublishNewMessageCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, ImageId imageId);
}

public class PublishNewMessageCommandFactory(INewImageProducer newImageProducer) : IPublishNewMessageCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, ImageId imageId)
    {
        return new PublishNewMessageCommand(newImageProducer, userId, imageId);
    }
}

public class PublishNewMessageCommand(INewImageProducer newImageProducer, UserId userId, ImageId imageId) : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return newImageProducer.SendAsync(new NewImage { BucketId = userId, ImageId = imageId }, cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
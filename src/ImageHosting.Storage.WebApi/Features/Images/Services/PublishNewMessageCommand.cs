using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Services;

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
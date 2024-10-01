using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Services;

public interface IPublishNewMessageCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, ImageId imageId, string imageName);
}

public class PublishNewMessageCommandFactory(INewImageProducer newImageProducer) : IPublishNewMessageCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, ImageId imageId, string name)
    {
        return new PublishNewMessageCommand(newImageProducer, userId, imageId, name);
    }
}

public class PublishNewMessageCommand(INewImageProducer newImageProducer, UserId userId, ImageId imageId, string name) : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return newImageProducer.SendAsync(new NewImage { Bucket = userId, ImageId = imageId, Name = name }, cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
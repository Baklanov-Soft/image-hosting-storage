using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.WebApi.Features.Images.Services;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IUploadFileHandler
{
    Task<ImageUploadedDTO> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden,
        DateTime uploadedAt,
        CancellationToken cancellationToken = default);
}

public class UploadFileHandler(
    IFileUploadCommandFactory fileUploadCommandFactory,
    IMetadataUploadCommandFactory metadataUploadCommandFactory,
    IPublishNewMessageCommandFactory publishNewMessageCommandFactory)
    : IUploadFileHandler
{
    public async Task<ImageUploadedDTO> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden,
        DateTime uploadedAt, CancellationToken cancellationToken = default)
    {
        await using var stream = formFile.OpenReadStream();

        var fileUploadCommand =
            fileUploadCommandFactory.CreateCommand(userId, imageId, formFile.Length, formFile.ContentType, stream);
        var metadataUploadCommand =
            metadataUploadCommandFactory.CreateCommand(userId, imageId, formFile.FileName, hidden, uploadedAt);
        var publishNewMessageCommand = publishNewMessageCommandFactory.CreateCommand(userId, imageId, formFile.FileName);

        var commands = new RollbackCommands();
        commands.Add(fileUploadCommand);
        commands.Add(metadataUploadCommand);
        commands.Add(publishNewMessageCommand);

        await commands.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        return new ImageUploadedDTO(imageId, userId, formFile.FileName, hidden, uploadedAt);
    }
}
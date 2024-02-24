using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Handlers;

public interface IUploadFileHandler
{
    Task<ImageUploadedResponse> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden, DateTime uploadedAt,
        CancellationToken cancellationToken = default);
}

public class UploadFileHandler(
    IFileUploadCommandFactory fileUploadCommandFactory,
    IMetadataUploadCommandFactory metadataUploadCommandFactory,
    IPublishNewMessageCommandFactory publishNewMessageCommandFactory)
    : IUploadFileHandler
{
    public async Task<ImageUploadedResponse> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden,
        DateTime uploadedAt, CancellationToken cancellationToken = default)
    {
        var fileUploadCommand = fileUploadCommandFactory.CreateCommand(userId, imageId, formFile);
        var metadataUploadCommand =
            metadataUploadCommandFactory.CreateCommand(userId, imageId, formFile.FileName, hidden, uploadedAt);
        var publishNewMessageCommand = publishNewMessageCommandFactory.CreateCommand(userId, imageId);

        var commands = new RollbackCommands();
        commands.Add(fileUploadCommand);
        commands.Add(metadataUploadCommand);
        commands.Add(publishNewMessageCommand);

        await commands.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        return new ImageUploadedResponse(imageId, userId, formFile.FileName, hidden, uploadedAt);
    }
}
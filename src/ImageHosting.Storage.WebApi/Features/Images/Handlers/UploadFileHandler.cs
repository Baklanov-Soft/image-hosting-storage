using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using ImageHosting.Storage.WebApi.Features.Images.Services;
using ImageHosting.Storage.WebApi.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IUploadFileHandler
{
    Task<ImageUploadedDto> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden, DateTime uploadedAt,
        CancellationToken cancellationToken = default);
}

public class UploadFileHandler(
    IFileUploadCommandFactory fileUploadCommandFactory,
    IMetadataUploadCommandFactory metadataUploadCommandFactory,
    IPublishNewMessageCommandFactory publishNewMessageCommandFactory)
    : IUploadFileHandler
{
    public async Task<ImageUploadedDto> UploadAsync(UserId userId, ImageId imageId, IFormFile formFile, bool hidden,
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

        return new ImageUploadedDto(imageId, userId, formFile.FileName, hidden, uploadedAt);
    }
}
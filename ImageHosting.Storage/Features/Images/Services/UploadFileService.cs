using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public class UploadFileService(
    IFileUploadCommandFactory fileUploadCommandFactory,
    IMetadataUploadCommandFactory metadataUploadCommandFactory)
    : IUploadFileService
{
    public async Task<ReadImageResponse> UploadAsync(Guid userId, Guid imageId, IFormFile formFile, bool hidden,
        DateTime uploadedAt, CancellationToken cancellationToken = default)
    {
        var fileUploadCommand = fileUploadCommandFactory.CreateCommand(userId, imageId, formFile);
        var metadataUploadCommand =
            metadataUploadCommandFactory.CreateCommand(userId, imageId, formFile.FileName, hidden, uploadedAt);

        var commands = new RollbackCommands();
        commands.Add(fileUploadCommand);
        commands.Add(metadataUploadCommand);

        await commands.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        return new ReadImageResponse(imageId, userId, formFile.FileName, hidden, uploadedAt);
    }
}
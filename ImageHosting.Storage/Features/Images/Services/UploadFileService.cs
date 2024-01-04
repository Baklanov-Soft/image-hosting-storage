using System;
using System.Collections.Generic;
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
    public async Task<ReadImageDto> UploadAsync(Guid id, string userId, IFormFile formFile, bool hidden,
        DateTime uploadedAt, List<string> categories, CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.ParseExact(userId, "D");

        var fileUploadCommand = fileUploadCommandFactory.CreateInstance(userId, formFile);
        var metadataUploadCommand =
            metadataUploadCommandFactory.CreateInstance(id, userGuid, formFile.FileName, hidden, uploadedAt,
                categories);

        var commands = new RollbackCommands();
        commands.Add(fileUploadCommand);
        commands.Add(metadataUploadCommand);

        await commands.ExecuteAsync(cancellationToken).ConfigureAwait(false);

        return new ReadImageDto(id, userGuid, formFile.FileName, hidden, uploadedAt, categories);
    }
}

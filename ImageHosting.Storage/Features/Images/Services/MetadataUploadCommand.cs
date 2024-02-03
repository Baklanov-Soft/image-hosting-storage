using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataUploadCommand(
    IMetadataService metadataService,
    Guid id,
    Guid userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt,
    List<string> categories)
    : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
        metadataService.WriteMetadataAsync(new ImageMetadata(id, objectName, userId, uploadedAt, hidden, categories),
            cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        metadataService.DeleteMetadataAsync(id, cancellationToken);
}

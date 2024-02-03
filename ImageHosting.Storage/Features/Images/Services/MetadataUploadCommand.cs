using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataUploadCommand(
    IMetadataService metadataService,
    Guid userId,
    Guid imageId,
    string objectName,
    bool hidden,
    DateTime uploadedAt)
    : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
        metadataService.WriteMetadataAsync(new ImageMetadata(imageId, objectName, userId, uploadedAt, hidden),
            cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        metadataService.DeleteMetadataAsync(imageId, cancellationToken);
}

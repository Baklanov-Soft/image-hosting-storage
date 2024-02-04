using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IMetadataUploadCommandFactory
{
    IRollbackCommand CreateCommand(Guid userId, Guid imageId, string objectName, bool hidden, DateTime uploadedAt);
}

public class MetadataUploadCommandFactory(IMetadataService metadataService) : IMetadataUploadCommandFactory
{
    public IRollbackCommand CreateCommand(Guid imageId, Guid userId, string objectName, bool hidden, DateTime uploadedAt)
    {
        return new MetadataUploadCommand(metadataService, imageId, userId, objectName, hidden, uploadedAt);
    }
}

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

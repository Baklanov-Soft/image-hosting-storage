using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IMetadataUploadCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, Guid imageId, string objectName, bool hidden, DateTime uploadedAt);
}

public class MetadataUploadCommandFactory(IMetadataService metadataService) : IMetadataUploadCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, Guid imageId, string objectName, bool hidden, DateTime uploadedAt)
    {
        return new MetadataUploadCommand(metadataService, userId, imageId, objectName, hidden, uploadedAt);
    }
}

public class MetadataUploadCommand(
    IMetadataService metadataService,
    UserId userId,
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

using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Services;

public interface IMetadataUploadCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, ImageId imageId, string objectName, bool hidden, DateTime uploadedAt);
}

public class MetadataUploadCommandFactory(IMetadataService metadataService) : IMetadataUploadCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, ImageId imageId, string objectName, bool hidden, DateTime uploadedAt)
    {
        return new MetadataUploadCommand(metadataService, userId, imageId, objectName, hidden, uploadedAt);
    }
}

public class MetadataUploadCommand(
    IMetadataService metadataService,
    UserId userId,
    ImageId imageId,
    string objectName,
    bool hidden,
    DateTime uploadedAt)
    : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
        metadataService.WriteMetadataAsync(new ImageMetadataDto(imageId, objectName, userId, uploadedAt, hidden),
            cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        metadataService.DeleteMetadataAsync(imageId, cancellationToken);
}

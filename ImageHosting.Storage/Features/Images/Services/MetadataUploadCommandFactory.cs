using System;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataUploadCommandFactory(IMetadataService metadataService) : IMetadataUploadCommandFactory
{
    public IRollbackCommand CreateCommand(Guid imageId, Guid userId, string objectName, bool hidden, DateTime uploadedAt)
    {
        return new MetadataUploadCommand(metadataService, imageId, userId, objectName, hidden, uploadedAt);
    }
}

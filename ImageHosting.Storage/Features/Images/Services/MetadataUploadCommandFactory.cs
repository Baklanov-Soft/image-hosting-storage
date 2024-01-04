using System;
using System.Collections.Generic;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public class MetadataUploadCommandFactory(IMetadataService metadataService) : IMetadataUploadCommandFactory
{
    public IRollbackCommand CreateInstance(Guid id, Guid userId, string objectName, bool hidden, DateTime uploadedAt,
        List<string> categories)
    {
        return new MetadataUploadCommand(metadataService, id, userId, objectName, hidden, uploadedAt, categories);
    }
}

using System;
using System.Collections.Generic;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IMetadataUploadCommandFactory
{
    IRollbackCommand CreateInstance(Guid id, Guid userId, string objectName, bool hidden, DateTime uploadedAt,
        List<string> categories);
}

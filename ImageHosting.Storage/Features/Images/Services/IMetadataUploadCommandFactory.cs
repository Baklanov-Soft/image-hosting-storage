using System;
using ImageHosting.Storage.Generic;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IMetadataUploadCommandFactory
{
    IRollbackCommand CreateCommand(Guid userId, Guid imageId, string objectName, bool hidden, DateTime uploadedAt);
}

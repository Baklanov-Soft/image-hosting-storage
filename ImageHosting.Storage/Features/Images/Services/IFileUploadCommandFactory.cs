using System;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IFileUploadCommandFactory
{
    IRollbackCommand CreateCommand(Guid userId, Guid imageId, IFormFile file);
}
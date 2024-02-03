using System;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public class FileUploadCommandFactory(IFileService fileService) : IFileUploadCommandFactory
{
    public IRollbackCommand CreateCommand(Guid userId, Guid imageId, IFormFile file)
    {
        return new FileUploadCommand(fileService, userId.ToString("D"), imageId.ToString("D"), file);
    }
}
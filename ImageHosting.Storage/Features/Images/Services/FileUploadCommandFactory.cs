using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public class FileUploadCommandFactory(IFileService fileService) : IFileUploadCommandFactory
{
    public IRollbackCommand CreateInstance(string userId, IFormFile file)
    {
        return new FileUploadCommand(fileService, userId, file);
    }
}

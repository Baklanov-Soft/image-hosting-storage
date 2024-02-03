using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public class FileUploadCommand(IFileService fileService, string userId, string imageId, IFormFile file) : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
        fileService.WriteFileAsync(new WriteFile(userId, imageId, file), cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        fileService.RemoveFileAsync(new RemoveFile(userId, imageId), cancellationToken);
}

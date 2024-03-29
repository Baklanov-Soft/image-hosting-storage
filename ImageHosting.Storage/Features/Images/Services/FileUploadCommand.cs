using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IFileUploadCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, ImageId imageId, IFormFile file);
}

public class FileUploadCommandFactory(IFileService fileService) : IFileUploadCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, ImageId imageId, IFormFile file)
    {
        return new FileUploadCommand(fileService, userId.ToString("D"), imageId.ToString("D"), file);
    }
}

public class FileUploadCommand(IFileService fileService, string userId, string imageId, IFormFile file) : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
        fileService.WriteFileAsync(new WriteFile(userId, imageId, file), cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        fileService.RemoveFileAsync(new RemoveFile(userId, imageId), cancellationToken);
}

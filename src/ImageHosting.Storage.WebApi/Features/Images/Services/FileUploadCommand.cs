using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Common;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Services;

public interface IFileUploadCommandFactory
{
    IRollbackCommand CreateCommand(UserId userId, ImageId imageId, long length, string contentType, Stream stream);
}

public class FileUploadCommandFactory(IFileService fileService) : IFileUploadCommandFactory
{
    public IRollbackCommand CreateCommand(UserId userId, ImageId imageId, long length, string contentType,
        Stream stream)
    {
        return new FileUploadCommand(fileService, userId.ToString("D"), imageId.ToString("D"), length, contentType,
            stream);
    }
}

public class FileUploadCommand(
    IFileService fileService,
    string userId,
    string imageId,
    long length,
    string contentType,
    Stream stream)
    : IRollbackCommand
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return fileService.WriteFileAsync(new WriteFileDTO(userId, imageId, length, contentType, stream),
            cancellationToken);
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        fileService.RemoveFileAsync(new RemoveFileDTO(userId, imageId), cancellationToken);
}
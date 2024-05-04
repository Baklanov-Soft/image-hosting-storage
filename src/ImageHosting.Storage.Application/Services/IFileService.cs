using ImageHosting.Storage.Application.DTOs;

namespace ImageHosting.Storage.Application.Services;

public interface IFileService
{
    Task<UploadFileDto> WriteFileAsync(WriteFileDto writeFileDto, CancellationToken cancellationToken = default);
    Task RemoveFileAsync(RemoveFileDto removeFileDto, CancellationToken cancellationToken = default);
}

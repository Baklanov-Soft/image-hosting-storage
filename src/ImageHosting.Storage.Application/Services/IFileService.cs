using ImageHosting.Storage.Application.DTOs;

namespace ImageHosting.Storage.Application.Services;

public interface IFileService
{
    Task<UploadFileDTO> WriteFileAsync(WriteFileDTO writeFileDto, CancellationToken cancellationToken = default);
    Task RemoveFileAsync(RemoveFileDTO removeFileDto, CancellationToken cancellationToken = default);
}

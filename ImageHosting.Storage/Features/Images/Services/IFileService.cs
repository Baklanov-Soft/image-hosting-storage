using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IFileService
{
    Task<UploadFileDto> WriteFileAsync(WriteFile writeFile, CancellationToken cancellationToken = default);
    Task RemoveFileAsync(RemoveFile removeFile, CancellationToken cancellationToken = default);
}

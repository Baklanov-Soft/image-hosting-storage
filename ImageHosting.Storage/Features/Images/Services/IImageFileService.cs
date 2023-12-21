using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IImageFileService
{
    Task<UploadResponseDto> WriteFileAsync(ImageFile imageFile, CancellationToken cancellationToken = default);
}

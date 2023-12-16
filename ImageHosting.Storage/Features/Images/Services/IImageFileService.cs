using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IImageFileService
{
    Task<PutObjectResponse> UploadImageAsync(UploadImageDto uploadImageDto,
        CancellationToken cancellationToken = default);
}

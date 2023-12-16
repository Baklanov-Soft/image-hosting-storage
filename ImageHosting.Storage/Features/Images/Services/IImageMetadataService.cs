namespace ImageHosting.Storage.Features.Images.Services;

public interface IImageMetadataService
{
    Task WriteMetadataAsync(UploadImageDto uploadImageDto,
        CancellationToken cancellationToken = default);

    Task<List<ReadImageDto>> GetAllowedImagesAsync(CancellationToken cancellationToken = default);
}
using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Application.Services;

public interface IMetadataService
{
    Task<ImageUploadedDTO> WriteMetadataAsync(ImageMetadataDTO imageMetadataDto,
        CancellationToken cancellationToken = default);

    Task DeleteMetadataAsync(ImageId id, CancellationToken cancellationToken = default);
}
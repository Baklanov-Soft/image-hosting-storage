using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Exceptions;

public class ImageMetadataNotFoundException(ImageId imageId) : Exception($"Image '{imageId}' not found.")
{
    public ImageId ImageId { get; } = imageId;
}
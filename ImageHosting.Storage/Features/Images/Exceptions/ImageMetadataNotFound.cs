using System;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class ImageMetadataNotFound(ImageId imageId) : Exception($"Image '{imageId}' not found.")
{
    public ImageId ImageId { get; } = imageId;
}
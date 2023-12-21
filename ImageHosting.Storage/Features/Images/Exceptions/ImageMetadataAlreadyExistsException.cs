using System;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class ImageMetadataAlreadyExistsException(string objectName)
    : Exception($"Image metadata with object name {objectName} already exists.")
{
    public string ObjectName { get; } = objectName;
}

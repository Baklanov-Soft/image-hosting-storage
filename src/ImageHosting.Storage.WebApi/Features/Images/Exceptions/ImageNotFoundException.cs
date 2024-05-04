using System;

namespace ImageHosting.Storage.WebApi.Features.Images.Exceptions;

public class ImageNotFoundException(string bucketName, string imageName)
    : Exception($"Image '{imageName}' not found in bucket '{bucketName}'.")
{
    public string BucketName { get; } = bucketName;
    public string ImageName { get; } = imageName;
}
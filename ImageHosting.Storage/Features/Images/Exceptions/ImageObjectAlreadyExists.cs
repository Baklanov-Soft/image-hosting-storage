using System;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class ImageObjectAlreadyExists(string bucketName, string imageId)
    : Exception($"Image id {imageId} already exists in bucket {bucketName}.")
{
    public string BucketName { get; } = bucketName;
    public string ImageId { get; } = imageId;
}

using System;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class ImageObjectAlreadyExists(string bucketName, string objectName)
    : Exception($"Image object {objectName} already exists in bucket {bucketName}.")
{
    public string BucketName { get; } = bucketName;
    public string ObjectName { get; } = objectName;
}

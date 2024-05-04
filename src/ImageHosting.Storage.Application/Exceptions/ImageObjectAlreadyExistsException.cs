namespace ImageHosting.Storage.Application.Exceptions;

public class ImageObjectAlreadyExistsException(string bucketName, string imageId)
    : Exception($"Image id {imageId} already exists in bucket {bucketName}.")
{
    public string BucketName { get; } = bucketName;
    public string ImageId { get; } = imageId;
}
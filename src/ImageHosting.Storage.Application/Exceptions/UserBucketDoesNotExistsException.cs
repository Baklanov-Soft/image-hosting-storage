namespace ImageHosting.Storage.Application.Exceptions;

public class UserBucketDoesNotExistsException(string userId) : Exception($"Bucket with id {userId} does not exists.")
{
    public string UserId { get; } = userId;
}
using System;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class UserBucketDoesNotExistsException(string userId) : Exception($"Bucket with id {userId} does not exists.")
{
    public string UserId { get; } = userId;
}
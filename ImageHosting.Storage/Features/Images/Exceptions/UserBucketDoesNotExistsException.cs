namespace ImageHosting.Storage.Features.Images.Exceptions;

public class UserBucketDoesNotExistsException : Exception
{
    public UserBucketDoesNotExistsException()
    {
    }

    public UserBucketDoesNotExistsException(string message) : base(message)
    {
    }

    public UserBucketDoesNotExistsException(string message, Exception inner) : base(message, inner)
    {
    }
}
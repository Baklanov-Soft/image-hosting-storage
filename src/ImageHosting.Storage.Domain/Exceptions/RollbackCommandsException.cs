namespace ImageHosting.Storage.Domain.Exceptions;

public class RollbackCommandsException : Exception
{
    public RollbackCommandsException()
    {
    }

    public RollbackCommandsException(string message) : base(message)
    {
    }

    public RollbackCommandsException(string message, Exception inner) : base(message, inner)
    {
    }
}

namespace ImageHosting.Storage.Domain.Common;

public interface IRollbackCommand
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

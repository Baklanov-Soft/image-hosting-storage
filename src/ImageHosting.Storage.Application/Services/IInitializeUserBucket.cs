namespace ImageHosting.Storage.Application.Services;

public interface IInitializeUserBucket
{
    Task CreateDefaultAsync(CancellationToken cancellationToken = default);
}
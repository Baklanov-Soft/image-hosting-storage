using System.Threading;
using System.Threading.Tasks;

namespace ImageHosting.Storage.Services;

public interface IInitializeUserBucket
{
    Task CreateDefaultAsync(CancellationToken cancellationToken = default);
}
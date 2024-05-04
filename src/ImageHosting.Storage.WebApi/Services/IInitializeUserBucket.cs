using System.Threading;
using System.Threading.Tasks;

namespace ImageHosting.Storage.WebApi.Services;

public interface IInitializeUserBucket
{
    Task CreateDefaultAsync(CancellationToken cancellationToken = default);
}
using System.Threading;
using System.Threading.Tasks;

namespace ImageHosting.Storage.Generic;

public interface IInitializeUserBucket
{
    Task CreateDefaultAsync(CancellationToken cancellationToken = default);
}
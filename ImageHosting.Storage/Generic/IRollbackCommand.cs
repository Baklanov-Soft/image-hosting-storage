using System.Threading;
using System.Threading.Tasks;

namespace ImageHosting.Storage.Generic;

public interface IRollbackCommand
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

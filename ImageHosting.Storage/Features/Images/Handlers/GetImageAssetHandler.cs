using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageHosting.Storage.Features.Images.Handlers;

public interface IGetImageAssetHandler
{
    Task<Stream> GetImageAsync(Guid imageId, CancellationToken cancellationToken);
}
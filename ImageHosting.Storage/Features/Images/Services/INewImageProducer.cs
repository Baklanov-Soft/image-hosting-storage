using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface INewImageProducer
{
    Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default);
}
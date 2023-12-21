using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IImageMetadataService
{
    Task WriteMetadataAsync(ImageMetadata imageMetadata, CancellationToken cancellationToken = default);
}

using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IMetadataService
{
    /// <summary>
    /// Saves metadata.
    /// </summary>
    /// <param name="imageMetadata">Metadata to write.</param>
    /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Created entity.</returns>
    Task<ReadImageResponse> WriteMetadataAsync(ImageMetadata imageMetadata, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete metadata.
    /// </summary>
    /// <param name="id">The id of metadata to delete.</param>
    /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>True if successful deleted.</returns>
    Task<bool> DeleteMetadataAsync(ImageId id, CancellationToken cancellationToken = default);
}

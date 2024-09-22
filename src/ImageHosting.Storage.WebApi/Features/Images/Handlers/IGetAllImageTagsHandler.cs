using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IGetAllImageTagsHandler
{
    Task<List<string>> GetTagsAsync(ImageId imageId, CancellationToken ct = default);
}
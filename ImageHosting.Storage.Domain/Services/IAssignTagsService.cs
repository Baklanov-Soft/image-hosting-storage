using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Domain.Services;

public interface IAssignTagsService
{
    Task AssignTagsAsync(Dictionary<ImageId, Dictionary<string, double>> categories,
        CancellationToken cancellationToken = default);
}
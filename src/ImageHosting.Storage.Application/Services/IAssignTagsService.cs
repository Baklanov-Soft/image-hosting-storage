using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Application.Services;

public interface IAssignTagsService
{
    Task AssignTagsAsync(Dictionary<ImageId, Dictionary<string, double>> categories,
        CancellationToken cancellationToken = default);
}
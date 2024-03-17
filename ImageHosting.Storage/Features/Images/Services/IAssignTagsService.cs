using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IAssignTagsService
{
    Task AssignTagsAsync(ImageId id, Dictionary<string, double> categories, double threshold,
        CancellationToken cancellationToken = default);
}
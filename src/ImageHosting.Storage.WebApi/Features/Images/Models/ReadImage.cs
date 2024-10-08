using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.WebApi.Features.Images.Models;

public class ReadImage
{
    public required ImageId Id { get; init; }
    public required string Name { get; init; }
    public required DateTime UploadedAt { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required bool Hidden { get; init; }
    public required Uri Asset { get; init; }
}
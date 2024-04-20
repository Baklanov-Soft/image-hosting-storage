using System.Text.Json.Serialization;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class NewImage
{
    [JsonPropertyName("bucketId")] public required UserId BucketId { get; init; }
    [JsonPropertyName("imageId")] public required ImageId ImageId { get; init; }
}

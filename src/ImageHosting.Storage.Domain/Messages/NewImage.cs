using System.Text.Json.Serialization;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Domain.Messages;

public class NewImage
{
    [JsonPropertyName("bucketId")] public required UserId BucketId { get; init; }
    [JsonPropertyName("imageId")] public required ImageId ImageId { get; init; }
}

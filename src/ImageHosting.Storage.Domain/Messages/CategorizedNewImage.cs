using System.Text.Json.Serialization;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Domain.Messages;

public class CategorizedNewImage
{
    [JsonPropertyName("imageId")] public required ImageId ImageId { get; init; }
    [JsonPropertyName("categories")] public required Dictionary<string, double> Categories { get; init; }
}
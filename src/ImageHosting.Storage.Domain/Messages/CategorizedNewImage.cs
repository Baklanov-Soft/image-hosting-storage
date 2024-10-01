using System.Text.Json.Serialization;

namespace ImageHosting.Storage.Domain.Messages;

public class CategorizedNewImage
{
    [JsonPropertyName("image")] public required NewImage Image { get; init; }
    [JsonPropertyName("categories")] public required Dictionary<string, double> Categories { get; init; }
}
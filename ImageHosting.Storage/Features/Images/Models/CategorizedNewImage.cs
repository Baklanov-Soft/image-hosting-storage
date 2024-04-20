using System.Collections.Generic;
using System.Text.Json.Serialization;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class CategorizedNewImage
{
    [JsonPropertyName("imageId")] public required ImageId ImageId { get; init; }
    [JsonPropertyName("categories")] public required Dictionary<string, double> Categories { get; init; }
}
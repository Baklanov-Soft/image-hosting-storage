using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class CategorizedNewImage
{
    [JsonPropertyName("imageId")] public required ImageId ImageId { get; init; }
    [JsonPropertyName("categories")] public required Dictionary<string, double> Categories { get; init; }

    public void Deconstruct(out ImageId imageId, out Dictionary<string, double> categories)
    {
        imageId = ImageId;
        categories = Categories;
    }
}

public class CategorizedNewImageDeserializer : IDeserializer<CategorizedNewImage>
{
    public CategorizedNewImage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<CategorizedNewImage>(data);
    }
}

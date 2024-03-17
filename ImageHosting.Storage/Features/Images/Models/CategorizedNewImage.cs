using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace ImageHosting.Storage.Features.Images.Models;

public class CategorizedNewImage : NewImage
{
    [JsonPropertyName("categories")] public required Dictionary<string, double> Categories { get; init; }
}

public class CategorizedNewImageDeserializer : IDeserializer<CategorizedNewImage>
{
    public CategorizedNewImage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<CategorizedNewImage>(data);
    }
}

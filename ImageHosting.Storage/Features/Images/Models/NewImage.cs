using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class NewImage
{
    [JsonPropertyName("bucketId")] public required UserId BucketId { get; init; }

    [JsonPropertyName("imageId")] public required Guid ImageId { get; init; }

    public class Serializer : ISerializer<NewImage>
    {
        public byte[] Serialize(NewImage data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data);
        }
    }
}
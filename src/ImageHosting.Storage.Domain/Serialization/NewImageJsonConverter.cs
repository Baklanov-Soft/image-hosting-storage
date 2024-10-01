using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.ValueTypes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ImageHosting.Storage.Domain.Serialization;

public class NewImageJsonConverter : JsonConverter<NewImage>
{
    public override NewImage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject)
            throw new JsonException("Expected start of object");

        UserId? bucket = null;
        ImageId? prefix = null;
        string? name = null;

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndObject)
                break;

            if (reader.TokenType is JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read(); // Move to the value

                switch (propertyName)
                {
                    case "bucket":
                        bucket = JsonSerializer.Deserialize<UserId>(ref reader, options);
                        break;

                    case "prefix":
                        prefix = JsonSerializer.Deserialize<ImageId>(ref reader, options);
                        break;

                    case "name":
                        name = reader.GetString();
                        break;

                    default:
                        throw new JsonException($"Unexpected property {propertyName}");
                }
            }
        }

        if (!bucket.HasValue || name == null || !prefix.HasValue)
        {
            throw new JsonException("Missing required properties");
        }

        return new NewImage
        {
            Bucket = bucket.Value,
            Name = name,
            ImageId = prefix.Value,
        };
    }

    public override void Write(Utf8JsonWriter writer, NewImage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("bucket");
        JsonSerializer.Serialize(writer, value.Bucket, options);

        writer.WritePropertyName("prefix");
        writer.WriteStringValue(value.Prefix);

        writer.WritePropertyName("name");
        writer.WriteStringValue(value.Name);

        writer.WriteEndObject();
    }
}
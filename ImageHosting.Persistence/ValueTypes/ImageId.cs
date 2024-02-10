using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHosting.Persistence.ValueTypes;

[JsonConverter(typeof(JsonConverter))]
public readonly record struct ImageId(Guid Id)
{
    public static ImageId ParseExact(string input, [StringSyntax("GuidFormat")] string format)
    {
        var guid = Guid.ParseExact(input, format);
        return new ImageId(guid);
    }
    
    public string ToString([StringSyntax("GuidFormat")] string? format)
    {
        return Id.ToString(format);
    }

    public class ValueConverter() : ValueConverter<ImageId, Guid>(userId => userId.Id, guid => new ImageId(guid));

    private class JsonConverter : JsonConverter<ImageId>
    {
        public override ImageId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ParseExact(reader.GetString()!, "D");
        }

        public override void Write(Utf8JsonWriter writer, ImageId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}

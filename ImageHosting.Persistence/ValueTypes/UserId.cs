using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHosting.Persistence.ValueTypes;

[JsonConverter(typeof(JsonConverter))]
public readonly record struct UserId(Guid Id)
{
    public static readonly UserId Empty = new(Guid.Empty);
    
    public static UserId ParseExact(string input, [StringSyntax("GuidFormat")] string format)
    {
        var guid = Guid.ParseExact(input, format);
        return new UserId(guid);
    }

    public string ToString([StringSyntax("GuidFormat")] string? format)
    {
        return Id.ToString(format);
    }

    public override string ToString()
    {
        return ToString("D");
    }

    public class ValueConverter() : ValueConverter<UserId, Guid>(userId => userId.Id, guid => new UserId(guid));

    private class JsonConverter : JsonConverter<UserId>
    {
        public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return ParseExact(reader.GetString()!, "D");
        }

        public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Id);
        }
    }
}

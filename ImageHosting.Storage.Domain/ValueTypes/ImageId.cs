using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ImageHosting.Storage.Domain.ValueTypes;

[JsonConverter(typeof(JsonConverter))]
public readonly record struct ImageId(Guid Id) : IParsable<ImageId>
{
    private static ImageId ParseExact(string input, [StringSyntax("GuidFormat")] string format)
    {
        var guid = Guid.ParseExact(input, format);
        return new ImageId(guid);
    }

    public override string ToString()
    {
        return ToString("D");
    }

    public string ToString([StringSyntax("GuidFormat")] string? format)
    {
        return Id.ToString(format);
    }

    public static ImageId Parse(string s, IFormatProvider? provider)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new ArgumentException("Invalid value for image id param.", nameof(s));
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out ImageId result)
    {
        if (s is not null)
        {
            result = ParseExact(s, "D");
            return true;
        }

        result = default;
        return false;
    }

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
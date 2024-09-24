using ImageHosting.Storage.Domain.ValueTypes;
using System.Text.Json.Serialization;

namespace ImageHosting.Storage.Domain.Messages;

public class NewImage
{
    [JsonPropertyName("bucket")] public UserId Bucket { get; set; }
    [JsonIgnore] public ImageId ImageId { get; set; }

    [JsonPropertyName("prefix")] public string Prefix => ImageId.ToString();
    [JsonPropertyName("image")] public string ImageName => "original.jpg";
}

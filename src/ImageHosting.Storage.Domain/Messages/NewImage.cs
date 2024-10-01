using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Domain.Messages;

public class NewImage
{
    public required UserId Bucket { get; init; }
    public string Prefix => ImageId.ToString();
    public required string Name { get; init; }
    public required ImageId ImageId { get; init; }
}

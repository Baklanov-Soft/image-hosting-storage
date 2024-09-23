using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Domain.Entities;

public class Image 
{
    public ImageId Id { get; set; }

    public UserId UserId { get; set; }

    public string ObjectName { get; set; } = null!;
    public bool Hidden { get; set; }
    public DateTime UploadedAt { get; set; }

    public HashSet<ImageTag> Tags { get; } = [];

    public void AddTag(string tagName)
    {
        Tags.Add(new ImageTag { Image = this, ImageId = Id, TagName = tagName });
    }

    public void AddTags(IEnumerable<string> tags)
    {
        foreach (var tag in tags)
        {
            AddTag(tag);
        }
    }
}
using ImageHosting.Persistence.Entities.Configuration;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.Entities;

[EntityTypeConfiguration(typeof(ImageConfiguration))]
public class Image 
{
    public ImageId Id { get; set; }

    public UserId UserId { get; set; }
    
    public required string ObjectName { get; set; }
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
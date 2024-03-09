using ImageHosting.Persistence.Entities.Configuration;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.Entities;

[EntityTypeConfiguration(typeof(ImageTagConfiguration))]
public class ImageTag : IEquatable<ImageTag>
{
    public required Image Image { get; init; }
    public ImageId ImageId { get; init; }
    public required string TagName { get; init; }

    public bool Equals(ImageTag? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ImageId.Equals(other.ImageId) && TagName == other.TagName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ImageTag)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ImageId, TagName);
    }
}
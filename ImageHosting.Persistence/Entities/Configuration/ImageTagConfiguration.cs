using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Persistence.Entities.Configuration;

public class ImageTagConfiguration : IEntityTypeConfiguration<ImageTag>
{
    public void Configure(EntityTypeBuilder<ImageTag> builder)
    {
        builder.Property(it => it.TagName).HasMaxLength(32);
        builder.Property(it => it.ImageId).HasConversion<ImageId.ValueConverter>();
        builder.HasKey(it => new { it.ImageId, it.TagName });
    }
}
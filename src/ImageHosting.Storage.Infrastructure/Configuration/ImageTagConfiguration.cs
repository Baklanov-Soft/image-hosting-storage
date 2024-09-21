using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Infrastructure.Configuration;

public class ImageTagConfiguration : IEntityTypeConfiguration<ImageTag>
{
    public void Configure(EntityTypeBuilder<ImageTag> builder)
    {
        builder.Property(it => it.TagName).HasMaxLength(32);
        builder.Property(it => it.ImageId).HasConversion<ImageIdConverter>();
        builder.HasKey(it => new { it.ImageId, it.TagName });
    }
}
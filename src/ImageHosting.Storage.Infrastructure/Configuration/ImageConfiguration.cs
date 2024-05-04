using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Infrastructure.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasConversion<ImageIdConverter>();
        builder.Property(i => i.ObjectName).HasMaxLength(200);
        builder.Property(i => i.Hidden).HasDefaultValue(false);
        builder.HasIndex(i => i.Hidden);
        builder.Property(i => i.UserId).HasConversion<UserIdConverter>();
        builder.Property(i => i.UploadedAt).HasDefaultValueSql("NOW()");

        builder.HasMany(i => i.Tags)
            .WithOne(it => it.Image)
            .HasForeignKey(it => it.ImageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
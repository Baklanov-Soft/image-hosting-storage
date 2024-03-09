using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Persistence.Entities.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasConversion<ImageId.ValueConverter>();
        builder.Property(i => i.ObjectName).HasMaxLength(200);
        builder.Property(i => i.Hidden).HasDefaultValue(false);
        builder.HasIndex(i => i.Hidden);
        builder.Property(i => i.UserId).HasConversion<UserId.ValueConverter>();

        builder.HasMany(i => i.Tags)
            .WithOne(it => it.Image)
            .HasForeignKey(it => it.ImageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
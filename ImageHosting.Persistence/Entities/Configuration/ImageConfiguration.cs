using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Persistence.Entities.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ObjectName).HasMaxLength(200);
        builder.HasIndex(i => i.ObjectName).IsUnique();
        builder.Property(i => i.Hidden).HasDefaultValue(false);
        builder.HasIndex(i => i.Hidden);
        builder.Property(i => i.Categories).HasColumnType("varchar(200)[]");
    }
}

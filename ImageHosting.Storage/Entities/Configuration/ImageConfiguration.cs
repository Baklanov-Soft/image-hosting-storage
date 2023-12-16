using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Entities.Configuration;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.ObjectName).HasMaxLength(200);
        builder.HasMany<ImageCategory>(ic=>ic.Categories)
            .WithOne(ic => ic.Image)
            .HasForeignKey(ic => ic.ImageId)
            .HasPrincipalKey(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

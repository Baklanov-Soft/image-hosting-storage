using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Entities.Configuration;

public class ImageCategoryConfiguration : IEntityTypeConfiguration<ImageCategory>
{
    public void Configure(EntityTypeBuilder<ImageCategory> builder)
    {
        builder.HasKey(ic => new {ic.ImageId, ic.CategoryId});
    }
}

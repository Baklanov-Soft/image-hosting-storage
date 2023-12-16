using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Persistence.Entities.Configuration;

public class ForbiddenCategoryConfiguration:IEntityTypeConfiguration<ForbiddenCategory>
{
    public void Configure(EntityTypeBuilder<ForbiddenCategory> builder)
    {
        builder.HasKey(c => c.Name);
        builder.Property(c => c.Name).HasMaxLength(200);
    }
}

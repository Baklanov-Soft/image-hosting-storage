using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Entities.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100);
        builder.Property(c => c.Forbidden).HasDefaultValue(false);
        builder.HasMany<ImageCategory>(ic => ic.Categories)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .HasPrincipalKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

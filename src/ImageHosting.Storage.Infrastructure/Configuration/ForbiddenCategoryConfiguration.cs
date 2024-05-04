using ImageHosting.Storage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageHosting.Storage.Infrastructure.Configuration;

public class ForbiddenCategoryConfiguration : IEntityTypeConfiguration<ForbiddenCategory>
{
    public void Configure(EntityTypeBuilder<ForbiddenCategory> builder)
    {
        builder.HasKey(c => c.Name);
        builder.Property(c => c.Name).HasMaxLength(200);
    }
}
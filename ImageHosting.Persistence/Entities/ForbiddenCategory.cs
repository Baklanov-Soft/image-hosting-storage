using ImageHosting.Persistence.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.Entities;

[EntityTypeConfiguration(typeof(ForbiddenCategoryConfiguration))]
public class ForbiddenCategory
{
    public string Name { get; set; } = null!;
}

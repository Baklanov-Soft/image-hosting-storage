using ImageHosting.Persistence.Entities.Configuration;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.Entities;

[EntityTypeConfiguration(typeof(ImageTagConfiguration))]
public class ImageTag
{
    public required Image Image { get; set; }
    public ImageId ImageId { get; init; }
    public required string TagName { get; init; }
}
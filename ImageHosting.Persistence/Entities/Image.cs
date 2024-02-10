using ImageHosting.Persistence.Entities.Configuration;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Persistence.Entities;

[EntityTypeConfiguration(typeof(ImageConfiguration))]
public class Image 
{
    public ImageId Id { get; set; }

    public UserId UserId { get; set; }
    
    public string ObjectName { get; set; } = null!;
    public bool Hidden { get; set; }
    public DateTime UploadedAt { get; set; }
    
    public List<string>? Categories { get; set; }
}

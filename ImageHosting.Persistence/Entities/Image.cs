namespace ImageHosting.Persistence.Entities;

public class Image 
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    
    public string ObjectName { get; set; } = null!;
    public bool Hidden { get; set; }
    public DateTime UploadedAt { get; set; }
    
    public List<string>? Categories { get; set; }
}

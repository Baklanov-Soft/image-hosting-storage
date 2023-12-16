namespace ImageHosting.Storage.Entities;

public class Image 
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string ObjectName { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public HashSet<ImageCategory> Categories { get; set; } = null!;
}

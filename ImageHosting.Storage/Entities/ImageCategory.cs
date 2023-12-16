namespace ImageHosting.Storage.Entities;

public class ImageCategory
{
    public Guid ImageId { get; set; }
    public Image Image { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

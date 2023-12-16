namespace ImageHosting.Storage.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Forbidden { get; set; }

    public HashSet<ImageCategory> Categories { get; set; } = null!;
}

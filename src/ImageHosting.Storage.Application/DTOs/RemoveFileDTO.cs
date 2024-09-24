namespace ImageHosting.Storage.Application.DTOs;

public class RemoveFileDTO(string userId, string imageId)
{
    public string Bucket { get; } = userId;
    public string ImageId { get; } = imageId;

    public string Prefix => ImageId.ToString();
    public string ImageName => "original.jpg";
    public string FullImageName => $"{Prefix}/{ImageName}";
}

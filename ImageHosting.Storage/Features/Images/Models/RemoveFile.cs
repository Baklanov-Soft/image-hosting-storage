namespace ImageHosting.Storage.Features.Images.Models;

public class RemoveFile(string userId, string imageId)
{
    public string UserId { get; } = userId;
    public string ImageId { get; } = imageId;
}

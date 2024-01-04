namespace ImageHosting.Storage.Features.Images.Models;

public class RemoveFile(string userId, string imageName)
{
    public string UserId { get; } = userId;
    public string ImageName { get; } = imageName;
}

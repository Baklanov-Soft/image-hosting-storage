namespace ImageHosting.Storage.Application.DTOs;

public class RemoveFileDto(string userId, string imageId)
{
    public string UserId { get; } = userId;
    public string ImageId { get; } = imageId;
}

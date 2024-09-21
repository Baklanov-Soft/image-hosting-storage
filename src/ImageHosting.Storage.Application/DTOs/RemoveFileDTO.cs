namespace ImageHosting.Storage.Application.DTOs;

public class RemoveFileDTO(string userId, string imageId)
{
    public string UserId { get; } = userId;
    public string ImageId { get; } = imageId;
}

using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Models;

public class WriteFile(string userId, string imageId, IFormFile file)
{
    public string UserId { get; } = userId;
    public string ImageId { get; } = imageId;
    public IFormFile File { get; } = file;
}
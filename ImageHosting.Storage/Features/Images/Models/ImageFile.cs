using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Models;

public class ImageFile(string userId, IFormFile image)
{
    public string UserId { get; } = userId;
    public IFormFile Image { get; } = image;
}

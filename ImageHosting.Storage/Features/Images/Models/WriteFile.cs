using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Models;

public class WriteFile(string userId, IFormFile file)
{
    public string UserId { get; } = userId;
    public IFormFile File { get; } = file;
}
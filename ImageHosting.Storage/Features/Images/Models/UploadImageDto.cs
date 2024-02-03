using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Models;

public class UploadImageDto
{
    [Required] public IFormFile Image { get; init; } = null!;
}

using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Features.Images;

public class UploadImageDto
{
    [Required] public Guid UserId { get; init; }
    [Required] public IFormFile Image { get; init; } = null!;
}

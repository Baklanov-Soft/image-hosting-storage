using System;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageHosting.Storage.Features.Images;

[ApiController]
[Route("[controller]")]
public class ImagesController(IUploadImageService uploadImageService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UploadResponseDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [Consumes(typeof(UploadImageDto), MediaTypeNames.Multipart.FormData)]
    public async Task<IActionResult> UploadAsync([FromForm] UploadImageDto uploadImageDto,
        CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString();
        var uploadedAt = DateTime.Now.ToUniversalTime();
        
        var uploadResponseDto =
            await uploadImageService.UploadAsync(id, userId, uploadImageDto.Image, uploadedAt, cancellationToken);

        return Ok(uploadResponseDto);
    }
}

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
public class ImagesController(IUploadFileService uploadFileService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ReadImageDto), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [Consumes(typeof(UploadImageDto), MediaTypeNames.Multipart.FormData)]
    public async Task<IActionResult> UploadAsync([FromForm] UploadImageDto uploadImageDto,
        CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString();
        var uploadedAt = DateTime.UtcNow;

        var uploadResponseDto =
            await uploadFileService.UploadAsync(id, userId, uploadImageDto.Image, hidden: false, uploadedAt,
                categories: [], cancellationToken);

        return Ok(uploadResponseDto);
    }
}

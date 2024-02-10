using System;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Persistence.ValueTypes;
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
    [ProducesResponseType(typeof(ReadImageResponse), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [Consumes(typeof(UploadImageRequest), MediaTypeNames.Multipart.FormData)]
    public async Task<IActionResult> UploadAsync([FromForm] UploadImageRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) switch
        {
            { } id => UserId.ParseExact(id, "D"),
            _ => UserId.Empty
        };
        var imageId = Guid.NewGuid();
        var uploadedAt = DateTime.UtcNow;

        var response = await uploadFileService.UploadAsync(userId, imageId, request.Image, hidden: false, uploadedAt,
            cancellationToken);

        return Ok(response);
    }
}
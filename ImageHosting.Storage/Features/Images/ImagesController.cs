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
public class ImagesController(IUploadFileHandler uploadFileHandler, IGetImageHandler getImageHandler) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ImageUploadedResponse), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    [Consumes(typeof(UploadImageRequest), MediaTypeNames.Multipart.FormData)]
    public async Task<IActionResult> Upload([FromForm] UploadImageRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) switch
        {
            { } id => UserId.ParseExact(id, "D"),
            _ => UserId.Empty
        };
        var imageId = new ImageId(Guid.NewGuid());
        var uploadedAt = DateTime.UtcNow;

        var response = await uploadFileHandler.UploadAsync(userId, imageId, request.Image, hidden: false, uploadedAt,
            cancellationToken);

        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReadImage), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var imageId = new ImageId(id);
        var image = await getImageHandler.GetAsync(imageId, cancellationToken);
        return Ok(image);
    }
}
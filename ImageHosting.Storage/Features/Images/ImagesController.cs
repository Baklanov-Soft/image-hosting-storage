using System.Net.Mime;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageFileService _imageFileService;
    private readonly IImageMetadataService _imageMetadataService;

    public ImagesController(IImageFileService imageFileService,IImageMetadataService imageMetadataService)
    {
        _imageFileService = imageFileService;
        _imageMetadataService = imageMetadataService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PutObjectResponse), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
    [Consumes(typeof(UploadImageDto), "multipart/form-data")]
    public async Task<IActionResult> UploadAsync([FromForm] UploadImageDto uploadImageDto,
        CancellationToken cancellationToken)
    {
        return Ok(await _imageFileService.UploadImageAsync(uploadImageDto, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllImages(CancellationToken cancellationToken)
    {
        return Ok(await _imageMetadataService.GetAllowedImagesAsync(cancellationToken));
    }
}

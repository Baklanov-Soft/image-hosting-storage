using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public class UploadImageService(IImageFileService imageFileService, IImageMetadataService imageMetadataService)
    : IUploadImageService
{
    public async Task<UploadResponseDto> UploadAsync(Guid id, string userId, IFormFile formFile, DateTime uploadedAt,
        CancellationToken cancellationToken = default)
    {
        var userGuid = Guid.ParseExact(userId, "d");
        var imageMetadata = new ImageMetadata(id, formFile.FileName, userGuid, uploadedAt);
        await imageMetadataService.WriteMetadataAsync(imageMetadata, cancellationToken);

        var imageFile = new ImageFile(userId, formFile);
        var uploadResponseDto = await imageFileService.WriteFileAsync(imageFile, cancellationToken);

        return uploadResponseDto;
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IUploadImageService
{
    Task<UploadResponseDto> UploadAsync(Guid id, string userId, IFormFile formFile, DateTime uploadedAt,
        CancellationToken cancellationToken = default);
}

using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IUploadFileService
{
    Task<ReadImageResponse> UploadAsync(Guid userId, Guid imageId, IFormFile formFile, bool hidden, DateTime uploadedAt,
        CancellationToken cancellationToken = default);
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IUploadFileService
{
    Task<ReadImageDto> UploadAsync(Guid id, string userId, IFormFile formFile, bool hidden, DateTime uploadedAt,
        List<string> categories, CancellationToken cancellationToken = default);
}

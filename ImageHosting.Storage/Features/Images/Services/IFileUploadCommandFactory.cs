using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Features.Images.Services;

public interface IFileUploadCommandFactory
{
    IRollbackCommand CreateInstance(string userId, IFormFile file);
}
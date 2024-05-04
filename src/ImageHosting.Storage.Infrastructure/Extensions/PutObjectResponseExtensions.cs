using ImageHosting.Storage.Application.DTOs;
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Infrastructure.Extensions;

public static class PutObjectResponseExtensions
{
    public static UploadFileDto ToDto(this PutObjectResponse putObjectResponse)
    {
        return new UploadFileDto(putObjectResponse.Etag[1..^1], putObjectResponse.ObjectName, putObjectResponse.Size);
    }
}
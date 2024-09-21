using ImageHosting.Storage.Application.DTOs;
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Infrastructure.Extensions;

public static class PutObjectResponseExtensions
{
    public static UploadFileDTO ToDTO(this PutObjectResponse putObjectResponse)
    {
        return new UploadFileDTO(putObjectResponse.Etag[1..^1], putObjectResponse.ObjectName, putObjectResponse.Size);
    }
}
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images.Models;

public class UploadFileDto(string? etag, string? objectName, long size)
{
    public string? Etag { get; } = etag;
    public string? ObjectName { get; } = objectName;
    public long Size { get; } = size;

    public static UploadFileDto From(PutObjectResponse putObjectResponse)
    {
        return new UploadFileDto(putObjectResponse.Etag[1..^1], putObjectResponse.ObjectName, putObjectResponse.Size);
    }
}

using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images.Models;

public class UploadResponseDto
{
    public string? Etag { get; init; }
    public string? ObjectName { get; init; }
    public long Size { get; init; }

    public static UploadResponseDto From(PutObjectResponse putObjectResponse)
    {
        return new UploadResponseDto
        {
            Etag = putObjectResponse.Etag[1..^1],
            ObjectName = putObjectResponse.ObjectName,
            Size = putObjectResponse.Size
        };
    }
}

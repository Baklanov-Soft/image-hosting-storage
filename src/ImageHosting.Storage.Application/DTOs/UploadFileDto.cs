namespace ImageHosting.Storage.Application.DTOs;

public class UploadFileDto(string? etag, string? objectName, long size)
{
    public string? Etag { get; } = etag;
    public string? ObjectName { get; } = objectName;
    public long Size { get; } = size;
}

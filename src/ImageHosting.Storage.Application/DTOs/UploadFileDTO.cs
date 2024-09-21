namespace ImageHosting.Storage.Application.DTOs;

public class UploadFileDTO(string? etag, string? objectName, long size)
{
    public string? Etag { get; } = etag;
    public string? ObjectName { get; } = objectName;
    public long Size { get; } = size;
}

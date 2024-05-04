namespace ImageHosting.Storage.Application.DTOs;

public class WriteFileDto(string userId, string imageId, long length, string contentType, Stream stream)
{
    public string UserId { get; } = userId;
    public string ImageId { get; } = imageId;
    public long Length { get; } = length;
    public string ContentType { get; } = contentType;
    public Stream Stream { get; } = stream;
}
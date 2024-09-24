namespace ImageHosting.Storage.Application.DTOs;

public class WriteFileDTO(string bucket, string imageId, long length, string contentType, Stream stream)
{
    public string Bucket { get; } = bucket;
    public string ImageId { get; } = imageId;
    public long Length { get; } = length;
    public string ContentType { get; } = contentType;
    public Stream Stream { get; } = stream;

    public string Prefix => ImageId.ToString();
    public string ImageName => "original.jpg";
    public string FullImageName => $"{Prefix}/{ImageName}";
}
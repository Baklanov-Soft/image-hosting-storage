namespace ImageHosting.Storage.WebApi.Features.Images.Models;

public class GetImageAssetResult
{
    public required MemoryStream Stream { get; init; }
    public required string ContentType { get; init; }
}
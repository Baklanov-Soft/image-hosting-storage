namespace ImageHosting.Storage.WebApi.Features.Images.Models;

public class AddTagsResponse
{
    public required IEnumerable<string> Tags { get; init; }
}
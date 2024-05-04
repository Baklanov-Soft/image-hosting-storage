using System.Collections.Generic;

namespace ImageHosting.Storage.WebApi.Features.Images.Models;

public class AddTagsCommand
{
    public required IEnumerable<string> Tags { get; init; }
}
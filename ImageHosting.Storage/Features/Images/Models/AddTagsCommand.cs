using System.Collections.Generic;

namespace ImageHosting.Storage.Features.Images.Models;

public class AddTagsCommand
{
    public required IEnumerable<string> Tags { get; init; }
}
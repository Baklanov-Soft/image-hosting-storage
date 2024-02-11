using System;
using System.Collections.Generic;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImage
{
    public required ImageId Id { get; init; }
    public required string Name { get; init; }
    public required DateTime UploadedAt { get; init; }
    public required IReadOnlyList<string> Categories { get; init; }
    public required bool Hidden { get; init; }
    public required Uri Url { get; init; }
}
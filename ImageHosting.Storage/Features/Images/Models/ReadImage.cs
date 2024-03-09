using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ImageHosting.Persistence.Entities;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImage
{
    public required ImageId Id { get; init; }
    public required string Name { get; init; }
    public required DateTime UploadedAt { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? Categories { get; init; }

    public required bool Hidden { get; init; }
    public required Uri Asset { get; init; }

    public static ReadImage From(Image i, Uri baseUri)
    {
        return new ReadImage
        {
            Id = i.Id,
            Name = i.ObjectName,
            UploadedAt = i.UploadedAt,
            Hidden = i.Hidden,
            Categories = i.Categories,
            Asset = new Uri(baseUri, $"api/v1/images/{i.Id}/asset")
        };
    }
}
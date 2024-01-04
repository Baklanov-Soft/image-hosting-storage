using System;
using System.Collections.Generic;
using ImageHosting.Persistence.Entities;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImageDto(
    Guid id,
    Guid userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt,
    IEnumerable<string> categories)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public string ObjectName { get; } = objectName;
    public bool Hidden { get; } = hidden;
    public DateTime UploadedAt { get; } = uploadedAt;
    public IEnumerable<string>? Categories { get; } = categories;

    public static ReadImageDto From(Image image)
    {
        return new ReadImageDto(image.Id, image.UserId, image.ObjectName, image.Hidden, image.UploadedAt,
            image.Categories ?? []);
    }
}
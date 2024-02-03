using System;
using ImageHosting.Persistence.Entities;

namespace ImageHosting.Storage.Features.Images.Models;

public class ImageMetadata(
    Guid id,
    string objectName,
    Guid userId,
    DateTime uploadedAt,
    bool hidden)
{
    public string ObjectName { get; } = objectName;

    internal Image ToEntity()
    {
        return new Image
        {
            Id = id,
            UserId = userId,
            ObjectName = ObjectName,
            UploadedAt = uploadedAt,
            Hidden = hidden
        };
    }
}

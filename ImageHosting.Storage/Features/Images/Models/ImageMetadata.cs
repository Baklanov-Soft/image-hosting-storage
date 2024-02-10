using System;
using ImageHosting.Persistence.Entities;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class ImageMetadata(
    Guid id,
    string objectName,
    UserId userId,
    DateTime uploadedAt,
    bool hidden)
{
    internal Image ToEntity()
    {
        return new Image
        {
            Id = id,
            UserId = userId,
            ObjectName = objectName,
            UploadedAt = uploadedAt,
            Hidden = hidden
        };
    }
}

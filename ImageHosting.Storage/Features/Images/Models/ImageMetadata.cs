using System;
using ImageHosting.Persistence.Entities;

namespace ImageHosting.Storage.Features.Images.Models;

public class ImageMetadata(Guid id, string objectName, Guid userId, DateTime uploadedAt)
{
    public string ObjectName { get; } = objectName;

    internal Image ToImage()
    {
        return new Image
        {
            Id = id,
            UserId = userId,
            ObjectName = ObjectName,
            UploadedAt = uploadedAt
        };
    }
}

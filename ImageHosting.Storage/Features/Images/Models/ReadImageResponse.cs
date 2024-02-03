using System;
using ImageHosting.Persistence.Entities;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImageResponse(
    Guid id,
    Guid userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public string ObjectName { get; } = objectName;
    public bool Hidden { get; } = hidden;
    public DateTime UploadedAt { get; } = uploadedAt;

    public static ReadImageResponse From(Image image)
    {
        return new ReadImageResponse(image.Id, image.UserId, image.ObjectName, image.Hidden, image.UploadedAt);
    }
}
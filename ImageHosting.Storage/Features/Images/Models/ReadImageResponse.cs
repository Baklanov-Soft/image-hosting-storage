using System;
using ImageHosting.Persistence.Entities;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImageResponse(
    Guid id,
    UserId userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt)
{
    public Guid Id { get; } = id;
    public UserId UserId { get; } = userId;
    public string ObjectName { get; } = objectName;
    public bool Hidden { get; } = hidden;
    public DateTime UploadedAt { get; } = uploadedAt;

    public static ReadImageResponse From(Image image)
    {
        return new ReadImageResponse(image.Id, image.UserId, image.ObjectName, image.Hidden, image.UploadedAt);
    }
}
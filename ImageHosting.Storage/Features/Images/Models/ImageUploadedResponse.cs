using System;
using ImageHosting.Persistence.Entities;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Models;

public class ImageUploadedResponse(
    ImageId id,
    UserId userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt)
{
    public ImageId Id { get; } = id;
    public UserId UserId { get; } = userId;
    public string ObjectName { get; } = objectName;
    public bool Hidden { get; } = hidden;
    public DateTime UploadedAt { get; } = uploadedAt;

    public static ImageUploadedResponse From(Image image)
    {
        return new ImageUploadedResponse(image.Id, image.UserId, image.ObjectName, image.Hidden, image.UploadedAt);
    }
}
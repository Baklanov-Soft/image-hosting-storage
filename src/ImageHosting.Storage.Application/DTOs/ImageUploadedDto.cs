using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Application.DTOs;

public class ImageUploadedDto(
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

    public static ImageUploadedDto From(Image image)
    {
        return new ImageUploadedDto(image.Id, image.UserId, image.ObjectName, image.Hidden, image.UploadedAt);
    }
}
using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.Domain.ValueTypes;

namespace ImageHosting.Storage.Application.DTOs;

public class ImageMetadataDTO(
    ImageId id,
    string objectName,
    UserId userId,
    DateTime uploadedAt,
    bool hidden)
{
    public Image ToEntity()
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

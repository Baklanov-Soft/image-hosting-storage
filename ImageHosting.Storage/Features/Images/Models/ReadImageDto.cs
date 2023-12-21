using System;
using System.Collections.Generic;

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImageDto(
    Guid id,
    Guid userId,
    string objectName,
    bool hidden,
    DateTime uploadedAt,
    List<string>? categories)
{
    public Guid Id { get; } = id;
    public Guid UserId { get; } = userId;
    public string ObjectName { get; } = objectName;
    public bool Hidden { get; } = hidden;
    public DateTime UploadedAt { get; } = uploadedAt;
    public List<string>? Categories { get; } = categories;
}

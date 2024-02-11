using System;
using ImageHosting.Persistence.ValueTypes;

namespace ImageHosting.Storage.Features.Images.Exceptions;

public class ImageNotFoundException(string message, ImageId id) : Exception(message)
{
    private const string MessageText = "Image not found.";

    public ImageNotFoundException(ImageId id) : this(MessageText, id)
    {
    }

    public ImageId Id { get; } = id;
}
using ImageHosting.Persistence.ValueTypes;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Logging;

public static partial class Log
{
    [LoggerMessage(
        EventId = WebApiEvents.WriteFile,
        Level = LogLevel.Information,
        Message = "File {ImageId} was successfully written to bucket {UserId}.")]
    public static partial void LogFileWritten(this ILogger logger, string imageId, string userId);

    [LoggerMessage(
        EventId = WebApiEvents.RemoveFile,
        Level = LogLevel.Information,
        Message = "File {ImageId} was successfully removed from bucket {UserId}.")]
    public static partial void LogFileRemoved(this ILogger logger, string imageId, string userId);

    [LoggerMessage(
        EventId = WebApiEvents.WriteMetadata,
        Level = LogLevel.Information,
        Message = "Metadata for image {ImageId} successfully written.")]
    public static partial void LogMetadataWritten(this ILogger logger, ImageId imageId);

    [LoggerMessage(
        EventId = WebApiEvents.MetadataDeleted,
        Level = LogLevel.Information,
        Message = "Metadata for image {ImageId} successfully deleted.")]
    public static partial void LogMetadataDeleted(this ILogger logger, ImageId imageId);

    [LoggerMessage(
        EventId = WebApiEvents.MessagePublished,
        Level = LogLevel.Information,
        Message = "Message published with imageId {ImageId} and bucketId {BucketId}.")]
    public static partial void LogMessagePublished(this ILogger logger, ImageId imageId, UserId bucketId);
}
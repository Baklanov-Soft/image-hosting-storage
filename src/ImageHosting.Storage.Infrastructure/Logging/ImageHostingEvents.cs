namespace ImageHosting.Storage.Infrastructure.Logging;

public static class ImageHostingEvents
{
    public const int WriteFile = 1000;
    public const int WriteMetadata = 1001;
    public const int MessagePublished = 1002;
    public const int RemoveFile = 1003;
    public const int MetadataDeleted = 1004;
    public const int ImageTagAssigned = 2000;
}
using ImageHosting.Storage.Entities;

namespace ImageHosting.Storage.Features.Images;

public static class QueryableOfImageExtensions
{
    public static IQueryable<ReadImageDto> ToReadImageDtos(this IQueryable<Image> images, string minioEndpoint)
    {
        return images.Select(i => new ReadImageDto($"{minioEndpoint}/{i.UserId}/{i.ObjectName}"));
    }
}

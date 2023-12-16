using ImageHosting.Persistence.Entities;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class QueryableOfImageExtensions
{
    public static IQueryable<ReadImageDto> ToReadImageDtos(this IQueryable<Image> images, string minioEndpoint)
    {
        return images.Select(i => new ReadImageDto($"{minioEndpoint}/{i.UserId}/{i.ObjectName}"));
    }
}

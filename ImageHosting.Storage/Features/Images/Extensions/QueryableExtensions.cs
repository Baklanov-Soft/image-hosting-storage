using System.Linq;
using ImageHosting.Persistence.Entities;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<ReadImageDto> ToReadImageDtos(this IQueryable<Image> images)
    {
        return images.Select(i => new ReadImageDto(i.Id, i.UserId, i.ObjectName, i.Hidden, i.UploadedAt, i.Categories));
    }
}

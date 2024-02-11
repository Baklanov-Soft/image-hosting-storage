using System;
using System.Linq;
using ImageHosting.Persistence.Entities;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<ReadImage> ToReadImages(this IQueryable<Image> queryable, Uri minioEndpoint)
    {
        return queryable.Select(i => new ReadImage
        {
            Id = i.Id,
            Name = i.ObjectName,
            UploadedAt = i.UploadedAt,
            Hidden = i.Hidden,
            Categories = i.Categories == null ? Array.Empty<string>() : i.Categories,
            Url = new Uri(minioEndpoint, $"{i.UserId}/{i.Id}")
        });
    }
}
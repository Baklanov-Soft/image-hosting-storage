using System;
using System.Linq;
using ImageHosting.Storage.Domain.Entities;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Storage.WebApi.Features.Images.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<ReadImage> ToReadImages(this IQueryable<Image> queryable, Uri baseUri)
    {
        return queryable.Select(i => new ReadImage
        {
            Id = i.Id,
            Name = i.ObjectName,
            UploadedAt = i.UploadedAt,
            Hidden = i.Hidden,
            Categories = i.Tags.Select(it => it.TagName).ToList(),
            Asset = new Uri(baseUri, $"api/v1/images/{i.Id}/asset")
        }).AsSplitQuery();
    }
}
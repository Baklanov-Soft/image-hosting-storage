using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.Entities;
using ImageHosting.Storage.Features.Images.Extensions;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Services;

public class ImageMetadataService : IImageMetadataService
{
    private readonly IImageHostingDbContext _dbContext;
    private readonly MinioOptions _options;

    public ImageMetadataService(IImageHostingDbContext dbContext, IOptions<MinioOptions> options)
    {
        _dbContext = dbContext;
        _options = options.Value;
    }

    public async Task WriteMetadataAsync(UploadImageDto uploadImageDto, CancellationToken cancellationToken = default)
    {
        var entity = new Image
        {
            UserId = uploadImageDto.UserId,
            ObjectName = uploadImageDto.Image.FileName,
        };
        _dbContext.Images.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ReadImageDto>> GetAllowedImagesAsync(CancellationToken cancellationToken = default)
    {
        var list = await _dbContext.Images
            .FromSql($"""
                      select i."UserId", i."ObjectName"
                      from "Images" i
                      where not i."Categories" && array(select "Name" from "ForbiddenCategories")
                      """)
            .ToReadImageDtos(_options.Endpoint)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return list;
    }
}

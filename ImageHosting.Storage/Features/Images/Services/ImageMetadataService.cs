using ImageHosting.Storage.DbContexts;
using ImageHosting.Storage.Entities;
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
            .Where(i => i.Categories.All(c => !c.Category.Forbidden))
            .ToReadImageDtos(_options.Endpoint)
            .ToListAsync(cancellationToken);
        return list;
    }
}

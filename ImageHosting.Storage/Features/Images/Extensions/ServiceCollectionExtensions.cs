using ImageHosting.Storage.Features.Images.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IImageFileService, ImageFileService>()
            .AddTransient<IImageMetadataService, ImageMetadataService>()
            .AddTransient<IUploadImageService, UploadImageService>();
    }
}

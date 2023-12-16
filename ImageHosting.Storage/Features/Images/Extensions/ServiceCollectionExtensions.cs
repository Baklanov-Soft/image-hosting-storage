using ImageHosting.Storage.Features.Images.Services;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageService(this IServiceCollection services)
    {
        return services.AddTransient<IImageFileService, ImageFileServices>();
    }
    
    public static IServiceCollection AddImageMetadataService(this IServiceCollection services)
    {
        return services.AddTransient<IImageMetadataService, ImageMetadataService>();
    }
}

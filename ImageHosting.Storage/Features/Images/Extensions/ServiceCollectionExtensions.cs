using ImageHosting.Storage.Features.Images.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IFileService, FileService>()
            .AddTransient<IFileUploadCommandFactory, FileUploadCommandFactory>()
            .AddTransient<IMetadataService, MetadataService>()
            .AddTransient<IMetadataUploadCommandFactory, MetadataUploadCommandFactory>()
            .AddTransient<IUploadFileService, UploadFileService>();
    }
}

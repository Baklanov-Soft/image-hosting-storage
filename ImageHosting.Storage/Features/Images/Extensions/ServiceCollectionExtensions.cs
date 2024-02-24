using ImageHosting.Storage.Features.Images.Handlers;
using ImageHosting.Storage.Features.Images.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddSingleton<INewImageProducer, NewImageProducer>();
        
        return services
            .AddTransient<IFileService, FileService>()
            .AddTransient<IFileUploadCommandFactory, FileUploadCommandFactory>()
            .AddTransient<IMetadataService, MetadataService>()
            .AddTransient<IMetadataUploadCommandFactory, MetadataUploadCommandFactory>()
            .AddTransient<IPublishNewMessageCommandFactory, PublishNewMessageCommandFactory>()
            .AddTransient<IUploadFileService, UploadFileService>();
    }
}
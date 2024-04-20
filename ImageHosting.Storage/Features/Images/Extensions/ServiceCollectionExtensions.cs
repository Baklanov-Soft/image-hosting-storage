using ImageHosting.Storage.Features.Images.Handlers;
using ImageHosting.Storage.Features.Images.Services;
using ImageHosting.Storage.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Storage.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddOptions<ImagesOptions>()
            .BindConfiguration(ImagesOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddScoped<INewImageProducer, NewImageProducer>();

        return services
            .AddTransient<IFileService, FileService>()
            .AddTransient<IFileUploadCommandFactory, FileUploadCommandFactory>()
            .AddTransient<IMetadataService, MetadataService>()
            .AddTransient<IMetadataUploadCommandFactory, MetadataUploadCommandFactory>()
            .AddTransient<IPublishNewMessageCommandFactory, PublishNewMessageCommandFactory>()
            .AddTransient<IUploadFileHandler, UploadFileHandler>()
            .AddTransient<IGetImageAssetHandler, GetImageAssetHandler>()
            .AddTransient<IGetImageHandler, GetImageHandler>()
            .AddTransient<IUpdateNameHandler, UpdateNameHandler>()
            .AddTransient<IAddTagsHandler, AddTagsHandler>()
            .AddTransient<IDeleteTagsHandler, DeleteTagsHandler>();
    }
}
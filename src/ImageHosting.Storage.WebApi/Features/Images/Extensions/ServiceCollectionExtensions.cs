using ImageHosting.Storage.WebApi.Features.Images.Handlers;
using ImageHosting.Storage.WebApi.Features.Images.Services;
using ImageHosting.Storage.WebApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Storage.WebApi.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddOptions<ImagesOptions>()
            .BindConfiguration(ImagesOptions.SectionName)
            .ValidateDataAnnotations();


        return services
            .AddTransient<IFileUploadCommandFactory, FileUploadCommandFactory>()
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
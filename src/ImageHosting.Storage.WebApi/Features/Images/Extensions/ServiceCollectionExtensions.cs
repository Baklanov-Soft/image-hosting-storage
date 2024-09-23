using ImageHosting.Storage.WebApi.Features.Images.Handlers;
using ImageHosting.Storage.WebApi.Features.Images.Services;
using ImageHosting.Storage.WebApi.Options;

namespace ImageHosting.Storage.WebApi.Features.Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageServices(this IServiceCollection services)
    {
        services.AddOptions<ImagesOptions>()
            .BindConfiguration(ImagesOptions.SectionName)
            .ValidateDataAnnotations();

        return services
            .AddScoped<IFileUploadCommandFactory, FileUploadCommandFactory>()
            .AddScoped<IMetadataUploadCommandFactory, MetadataUploadCommandFactory>()
            .AddScoped<IPublishNewMessageCommandFactory, PublishNewMessageCommandFactory>()
            .AddScoped<IUploadFileHandler, UploadFileHandler>()
            .AddScoped<IGetImageAssetHandler, GetImageAssetHandler>()
            .AddScoped<IGetImageHandler, GetImageHandler>()
            .AddScoped<IUpdateNameHandler, UpdateNameHandler>()
            .AddScoped<IAddTagsHandler, AddTagsHandler>()
            .AddScoped<IDeleteTagsHandler, DeleteTagsHandler>()
            .AddScoped<IGetAllImageTagsHandler, GetAllImageTagsHandler>()
            .AddScoped<ISetHidenessHandler, SetHidenessHandler>();
    }
}
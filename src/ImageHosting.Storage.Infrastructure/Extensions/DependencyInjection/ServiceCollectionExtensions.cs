using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Options;
using ImageHosting.Storage.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageHosting.Storage.Infrastructure.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageHostingDbContext(this IServiceCollection serviceCollection,
        string connectionStringKey)
    {
        return serviceCollection
            .AddDbContext<IImageHostingDbContext, ImageHostingDbContext>((provider, optionsBuilder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(connectionStringKey);
                optionsBuilder.UseNpgsql(connectionString);
            });
    }
    
    public static IServiceCollection AddAssignTagsService(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<AssignTagsOptions>()
            .BindConfiguration(AssignTagsOptions.SectionName)
            .ValidateOnStart();
        
        return serviceCollection.AddScoped<IAssignTagsService, AssignTagsService>();
    }
    
    public static IServiceCollection AddMinioServices(this IServiceCollection services)
    {
        services.AddOptions<MinioOptions>()
            .BindConfiguration(MinioOptions.SectionName)
            .ValidateDataAnnotations();

        services.TryAddSingleton<IMinioClientFactory, MinioClientFactory>();
        services.TryAddTransient(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient());

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INewImageProducer, NewImageProducer>();

        return serviceCollection
            .AddTransient<IFileService, FileService>()
            .AddTransient<IMetadataService, MetadataService>();
    }
}

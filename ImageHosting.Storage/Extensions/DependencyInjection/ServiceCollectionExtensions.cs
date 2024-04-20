using ImageHosting.Storage.Models;
using ImageHosting.Storage.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageHosting.Storage.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinioServices(this IServiceCollection services)
    {
        services.AddOptions<MinioOptions>()
            .BindConfiguration(MinioOptions.SectionName)
            .ValidateDataAnnotations();

        services.TryAddSingleton<IMinioClientFactory, MinioClientFactory>();
        services.TryAddSingleton(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient());

        return services;
    }

    public static IServiceCollection AddKafkaServices(this IServiceCollection services)
    {
        services.AddOptions<KafkaOptions>()
            .BindConfiguration(KafkaOptions.SectionName)
            .ValidateDataAnnotations();
        
        return services;
    }

    public static IServiceCollection AddInitializeUserBucket(this IServiceCollection services)
    {
        return services.AddScoped<IInitializeUserBucket, InitializeUserBucket>();
    }
}

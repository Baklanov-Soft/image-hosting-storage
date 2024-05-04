using ImageHosting.Storage.WebApi.Models;
using ImageHosting.Storage.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageHosting.Storage.WebApi.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaOptions(this IServiceCollection services)
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

using ImageHosting.Storage.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinio(this IServiceCollection services)
    {
        services.AddOptions<MinioOptions>()
            .BindConfiguration(MinioOptions.SectionName)
            .ValidateDataAnnotations();

        services.TryAddSingleton<IMinioClientFactory, MinioClientFactory>();
        services.TryAddSingleton(sp => sp.GetRequiredService<IMinioClientFactory>().CreateClient());

        return services;
    }

    public static OptionsBuilder<KafkaOptions> AddKafkaOptions(this IServiceCollection services)
    {
        return services.AddOptions<KafkaOptions>()
            .BindConfiguration(KafkaOptions.SectionName)
            .ValidateDataAnnotations();
    }

    public static IServiceCollection AddInitializeUserBucket(this IServiceCollection services)
    {
        return services.AddTransient<IInitializeUserBucket, InitializeUserBucket>();
    }
}

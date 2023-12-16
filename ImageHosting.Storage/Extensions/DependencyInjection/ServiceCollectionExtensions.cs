using ImageHosting.Storage.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
}

using ImageHosting.Storage.Domain.Services;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Options;
using ImageHosting.Storage.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        return serviceCollection.AddScoped<IAssignTagsService, AssignTagsService>();
    }
    
    public static IServiceCollection AddAssignTagsOptions(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddOptions<AssignTagsOptions>()
            .BindConfiguration(AssignTagsOptions.SectionName)
            .ValidateOnStart();

        return serviceCollection;
    }
}

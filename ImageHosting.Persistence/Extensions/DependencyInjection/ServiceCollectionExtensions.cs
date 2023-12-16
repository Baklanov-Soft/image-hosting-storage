using ImageHosting.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageHosting.Persistence.Extensions.DependencyInjection;

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
}

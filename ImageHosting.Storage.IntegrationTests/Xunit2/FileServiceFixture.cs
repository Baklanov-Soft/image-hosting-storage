using ImageHosting.Storage.Features.Images.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Testcontainers.Minio;

namespace ImageHosting.Storage.IntegrationTests.Xunit2;

[UsedImplicitly]
public class FileServiceFixture : IAsyncLifetime
{
    private readonly MinioContainer _minioContainer = new MinioBuilder().Build();

    public IMinioClient MinioClient { get; private set; } = null!;
    public FileService FileService { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _minioContainer.StartAsync();

        var accessKey = _minioContainer.GetAccessKey();
        var secretKey = _minioContainer.GetSecretKey();
        var endpoint = $"{_minioContainer.Hostname}:{_minioContainer.GetMappedPublicPort(9000)}";
        
        var serviceProvider = ConfigureServices(accessKey, secretKey, endpoint);

        MinioClient = serviceProvider.GetRequiredService<IMinioClient>();
        FileService = serviceProvider.GetRequiredService<FileService>();
    }

    public Task DisposeAsync()
    {
        return _minioContainer.DisposeAsync().AsTask();
    }

    private static ServiceProvider ConfigureServices(string accessKey, string secretKey, string endpoint)
    {
        var services = new ServiceCollection();

        services.AddMinio(client => client.WithCredentials(accessKey, secretKey).WithEndpoint(endpoint).WithSSL(false));
        services.AddSingleton<FileService>();

        return services.BuildServiceProvider();
    }
}

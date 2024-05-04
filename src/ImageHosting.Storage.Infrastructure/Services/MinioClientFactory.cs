using ImageHosting.Storage.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;

namespace ImageHosting.Storage.Infrastructure.Services;

public class MinioClientFactory(IOptions<MinioOptions> options) : IMinioClientFactory
{
    private readonly MinioOptions _options = options.Value;

    public IMinioClient CreateClient()
    {
        return new MinioClient()
            .WithEndpoint(_options.Endpoint)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(_options.Secure)
            .Build();
    }
}

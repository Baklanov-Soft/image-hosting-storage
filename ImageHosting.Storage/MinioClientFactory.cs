using Microsoft.Extensions.Options;
using Minio;

namespace ImageHosting.Storage;

public class MinioClientFactory : IMinioClientFactory
{
    private readonly MinioOptions _options;

    public MinioClientFactory(IOptions<MinioOptions> options)
    {
        _options = options.Value;
    }

    public IMinioClient CreateClient()
    {
        return new MinioClient()
            .WithEndpoint(_options.Endpoint)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(_options.Secure)
            .Build();
    }
}

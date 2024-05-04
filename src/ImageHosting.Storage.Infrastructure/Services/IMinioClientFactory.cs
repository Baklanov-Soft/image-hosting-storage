using Minio;

namespace ImageHosting.Storage.Infrastructure.Services;

public interface IMinioClientFactory
{
    IMinioClient CreateClient();
}

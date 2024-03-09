using Minio;

namespace ImageHosting.Storage.Services;

public interface IMinioClientFactory
{
    IMinioClient CreateClient();
}

using Minio;

namespace ImageHosting.Storage;

public interface IMinioClientFactory
{
    IMinioClient CreateClient();
}

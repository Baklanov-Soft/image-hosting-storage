using Minio;

namespace ImageHosting.Storage.Generic;

public interface IMinioClientFactory
{
    IMinioClient CreateInstance();
}

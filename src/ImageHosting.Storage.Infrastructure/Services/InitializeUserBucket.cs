using ImageHosting.Storage.Application.Services;
using Minio;
using Minio.DataModel.Args;

namespace ImageHosting.Storage.Infrastructure.Services;

public class InitializeUserBucket(IMinioClient minioClient) : IInitializeUserBucket
{
    public async Task CreateDefaultAsync(CancellationToken cancellationToken = default)
    {
        var bucket = Guid.Empty.ToString();
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucket);
        var makeBucketArgs = new MakeBucketArgs().WithBucket(bucket);

        var isExists = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!isExists)
        {
            await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken).ConfigureAwait(false);
        }
    }
}
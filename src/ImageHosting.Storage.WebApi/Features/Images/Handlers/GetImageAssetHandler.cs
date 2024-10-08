using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.WebApi.Features.Images.Exceptions;
using ImageHosting.Storage.WebApi.Features.Images.Models;
using Minio;
using Minio.DataModel.Args;

namespace ImageHosting.Storage.WebApi.Features.Images.Handlers;

public interface IGetImageAssetHandler
{
    Task<GetImageAssetResult> GetImageAsync(UserId userId, string objectName,
        CancellationToken cancellationToken = default);
}

public class GetImageAssetHandler(IMinioClient minioClient) : IGetImageAssetHandler
{
    public async Task<GetImageAssetResult> GetImageAsync(UserId userId, string objectName,
        CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        var bucket = userId.ToString();

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName);
        var statObject = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken).ConfigureAwait(false);
        if (statObject.Size <= 0)
        {
            throw new ImageNotFoundException(bucket, objectName);
        }

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName)
            .WithLength(statObject.Size)
            .WithCallbackStream(async (stream, token) =>
            {
                await stream.CopyToAsync(memoryStream, token).ConfigureAwait(false);
                await stream.FlushAsync(token).ConfigureAwait(false);
                memoryStream.Position = 0;
                await stream.DisposeAsync().ConfigureAwait(false);
            });

        var objectStat = await minioClient.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);

        return new GetImageAssetResult
        {
            Stream = memoryStream,
            ContentType = objectStat.ContentType
        };
    }
}
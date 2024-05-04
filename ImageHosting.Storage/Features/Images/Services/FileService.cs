using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Logging;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace ImageHosting.Storage.Features.Images.Services;

public class FileService(IMinioClient minioClient, ILogger<FileService> logger) : IFileService
{
    public async Task<UploadFileDto> WriteFileAsync(WriteFile writeFile,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(writeFile.UserId);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(writeFile.UserId);
        }

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(writeFile.UserId)
            .WithObject(writeFile.ImageId);
        var objectStat = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken).ConfigureAwait(false);
        if (objectStat.Size > 0)
        {
            throw new ImageObjectAlreadyExistsException(writeFile.UserId, writeFile.ImageId);
        }

        var stream = writeFile.File.OpenReadStream();

        await using (stream.ConfigureAwait(false))
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(writeFile.UserId)
                .WithObjectSize(writeFile.File.Length)
                .WithContentType(writeFile.File.ContentType)
                .WithStreamData(stream)
                .WithObject(writeFile.ImageId);

            var putObjectResponse =
                await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

            logger.LogFileWritten(writeFile.ImageId, writeFile.UserId);
            return UploadFileDto.From(putObjectResponse);
        }
    }

    public async Task RemoveFileAsync(RemoveFile removeFile, CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(removeFile.UserId);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(removeFile.UserId);
        }

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(removeFile.UserId)
            .WithObject(removeFile.ImageId);
        await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken).ConfigureAwait(false);

        logger.LogFileRemoved(removeFile.ImageId, removeFile.UserId);
    }
}
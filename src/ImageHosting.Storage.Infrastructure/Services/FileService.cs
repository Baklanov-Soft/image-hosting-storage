using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Exceptions;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Infrastructure.Extensions;
using ImageHosting.Storage.Infrastructure.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Microsoft.Extensions.Logging;

namespace ImageHosting.Storage.Infrastructure.Services;

public class FileService(IMinioClient minioClient, ILogger<FileService> logger) : IFileService
{
    public async Task<UploadFileDTO> WriteFileAsync(WriteFileDTO writeFileDto,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(writeFileDto.Bucket);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(writeFileDto.Bucket);
        }

        var isObjectExists = await IsObjectExistsAsync(writeFileDto.Bucket, writeFileDto.FullImageName, cancellationToken)
            .ConfigureAwait(false);
        if (isObjectExists)
        {
            throw new ImageObjectAlreadyExistsException(writeFileDto.Bucket, writeFileDto.ImageId);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(writeFileDto.Bucket)
            .WithObjectSize(writeFileDto.Length)
            .WithContentType(writeFileDto.ContentType)
            .WithStreamData(writeFileDto.Stream)
            .WithObject(writeFileDto.FullImageName);

        var putObjectResponse =
            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

        logger.LogFileWritten(writeFileDto.ImageId, writeFileDto.Bucket);
        return putObjectResponse.ToDTO();
    }

    private async Task<bool> IsObjectExistsAsync(string bucketId, string objectName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketId)
                .WithObject(objectName);
            _ = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken).ConfigureAwait(false);

            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
    }

    public async Task RemoveFileAsync(RemoveFileDTO removeFileDto, CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(removeFileDto.Bucket);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(removeFileDto.Bucket);
        }

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(removeFileDto.Bucket)
            .WithObject(removeFileDto.FullImageName);
        await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken).ConfigureAwait(false);

        logger.LogFileRemoved(removeFileDto.ImageId, removeFileDto.Bucket);
    }
}
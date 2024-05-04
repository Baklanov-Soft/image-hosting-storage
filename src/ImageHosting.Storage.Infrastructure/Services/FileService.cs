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
    public async Task<UploadFileDto> WriteFileAsync(WriteFileDto writeFileDto,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(writeFileDto.UserId);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(writeFileDto.UserId);
        }

        var isObjectExists = await IsObjectExistsAsync(writeFileDto.UserId, writeFileDto.ImageId, cancellationToken);
        if (isObjectExists)
        {
            throw new ImageObjectAlreadyExistsException(writeFileDto.UserId, writeFileDto.ImageId);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(writeFileDto.UserId)
            .WithObjectSize(writeFileDto.Length)
            .WithContentType(writeFileDto.ContentType)
            .WithStreamData(writeFileDto.Stream)
            .WithObject(writeFileDto.ImageId);

        var putObjectResponse =
            await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

        logger.LogFileWritten(writeFileDto.ImageId, writeFileDto.UserId);
        return putObjectResponse.ToDto();
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

    public async Task RemoveFileAsync(RemoveFileDto removeFileDto, CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(removeFileDto.UserId);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(removeFileDto.UserId);
        }

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(removeFileDto.UserId)
            .WithObject(removeFileDto.ImageId);
        await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken).ConfigureAwait(false);

        logger.LogFileRemoved(removeFileDto.ImageId, removeFileDto.UserId);
    }
}
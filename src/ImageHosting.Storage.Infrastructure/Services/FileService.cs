using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Exceptions;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Infrastructure.Extensions;
using Minio;
using Minio.DataModel.Args;

namespace ImageHosting.Storage.Infrastructure.Services;

public class FileService(IMinioClient minioClient) : IFileService
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

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(writeFileDto.UserId)
            .WithObject(writeFileDto.ImageId);
        var objectStat = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken).ConfigureAwait(false);
        if (objectStat.Size > 0)
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

        return putObjectResponse.ToDto();
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
    }
}
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using Minio;
using Minio.DataModel.Args;

namespace ImageHosting.Storage.Features.Images.Services;

public class ImageFileService(IMinioClient minioClient) : IImageFileService
{
    public async Task<UploadResponseDto> WriteFileAsync(ImageFile imageFile,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(imageFile.UserId);
        var foundBucket =
            await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!foundBucket)
        {
            throw new UserBucketDoesNotExistsException(imageFile.UserId);
        }

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(imageFile.UserId)
            .WithObject(imageFile.Image.FileName);
        var objectStat = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken).ConfigureAwait(false);
        if (objectStat.Size > 0)
        {
            throw new ImageObjectAlreadyExists(imageFile.UserId, imageFile.Image.FileName);
        }

        var stream = imageFile.Image.OpenReadStream();

        await using (stream.ConfigureAwait(false))
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(imageFile.UserId)
                .WithObjectSize(imageFile.Image.Length)
                .WithContentType(imageFile.Image.ContentType)
                .WithStreamData(stream)
                .WithObject(imageFile.Image.FileName);

            var putObjectResponse =
                await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

            return UploadResponseDto.From(putObjectResponse);
        }
    }
}

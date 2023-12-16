using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images.Services;

public class ImageFileServices(IMinioClient minioClient) : IImageFileService
{
    public async Task<PutObjectResponse> UploadImageAsync(UploadImageDto uploadImageDto,
        CancellationToken cancellationToken = default)
    {
        var bucketName = uploadImageDto.UserId.ToString();

        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        var found = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!found)
        {
            throw new UserBucketDoesNotExistsException();
        }

        var stream = uploadImageDto.Image.OpenReadStream();

        await using (stream.ConfigureAwait(false))
        {
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObjectSize(uploadImageDto.Image.Length)
                .WithContentType(uploadImageDto.Image.ContentType)
                .WithStreamData(stream)
                .WithObject(uploadImageDto.Image.FileName);

            return await minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
        }
    }
}

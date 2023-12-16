using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;

namespace ImageHosting.Storage.Features.Images.Services;

public class ImageFileServices : IImageFileService
{
    private readonly IMinioClient _minioClient;

    public ImageFileServices(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<PutObjectResponse> UploadImageAsync(UploadImageDto uploadImageDto,
        CancellationToken cancellationToken = default)
    {
        var bucketName = uploadImageDto.UserId.ToString();

        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        var found = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken).ConfigureAwait(false);
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

            return await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
        }
    }
}

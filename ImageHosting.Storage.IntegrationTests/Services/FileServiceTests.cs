using FluentAssertions;
using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using Minio;

namespace ImageHosting.Storage.IntegrationTests.Services;

public class FileServiceTests(FileServiceFixture fileServiceFixture) : IClassFixture<FileServiceFixture>
{
    private readonly IMinioClient _minioClient = fileServiceFixture.MinioClient;
    private readonly FileService _fileService = fileServiceFixture.FileService;

    [Fact]
    public async Task Write_file_without_bucket()
    {
        var userId = Guid.NewGuid().ToString();
        var formFile = FileFixture.GetPlainTextFormFile();

        var act = () => _fileService.WriteFileAsync(new WriteFile(userId, formFile));

        await act.Should().ThrowAsync<UserBucketDoesNotExistsException>().WithMessage("Bucket with id*");
    }
    //todo: add more cases
}

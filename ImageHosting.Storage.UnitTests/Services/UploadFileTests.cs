using ImageHosting.Storage.Features.Images.Exceptions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute.ExceptionExtensions;

namespace ImageHosting.Storage.UnitTests.Services;

public class UploadFileTests
{
    [Fact]
    public async Task Successfully_upload_file()
    {
        var imageId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var fileService = Substitute.For<IFileService>();
        var metadataService = Substitute.For<IMetadataService>();
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var sut = new UploadFileService(fileUploadCommandFactory, metadataUploadCommandFactory);

        var image = await sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.ReceivedWithAnyArgs().WriteMetadataAsync(default!);
        image.Id.Should().Be(imageId);
        image.UserId.Should().Be(userId);
        image.ObjectName.Should().Be(formFile.FileName);
        image.Hidden.Should().Be(hidden);
        image.UploadedAt.Should().Be(uploadedAt);
    }

    [Fact]
    public async Task Upload_already_exists_file()
    {
        var imageId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var metadataService = Substitute.For<IMetadataService>();
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var fileService = Substitute.For<IFileService>();
        fileService.WriteFileAsync(Arg.Any<WriteFile>())
            .ThrowsAsync(new ImageObjectAlreadyExists(userId.ToString(), formFile.FileName));
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var sut = new UploadFileService(fileUploadCommandFactory, metadataUploadCommandFactory);

        var act = () => sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await act.Should().ThrowAsync<Exception>().WithInnerException(typeof(ImageObjectAlreadyExists));
        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
    }

    [Fact]
    public async Task Write_already_exists_metadata()
    {
        var imageId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var metadataService = Substitute.For<IMetadataService>();
        metadataService.WriteMetadataAsync(Arg.Any<ImageMetadata>())
            .ThrowsAsync<DbUpdateException>();
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var fileService = Substitute.For<IFileService>();
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var sut = new UploadFileService(fileUploadCommandFactory, metadataUploadCommandFactory);

        var act = () => sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await act.Should().ThrowAsync<Exception>().WithInnerException(typeof(DbUpdateException));
        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.ReceivedWithAnyArgs().WriteMetadataAsync(default!);
        await fileService.ReceivedWithAnyArgs().RemoveFileAsync(default!);
    }
}
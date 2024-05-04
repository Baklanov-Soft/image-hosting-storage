using Confluent.Kafka;
using ImageHosting.Storage.Application.DTOs;
using ImageHosting.Storage.Application.Exceptions;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Exceptions;
using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.WebApi.Features.Images.Handlers;
using ImageHosting.Storage.WebApi.Features.Images.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute.ExceptionExtensions;

namespace ImageHosting.Storage.UnitTests.Services;

public class UploadFileTests
{
    [Fact]
    public async Task Successfully_upload_file()
    {
        var imageId = new ImageId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var fileService = Substitute.For<IFileService>();
        var metadataService = Substitute.For<IMetadataService>();
        var newImageProducer = Substitute.For<INewImageProducer>();
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var publishNewMessageCommandFactory = new PublishNewMessageCommandFactory(newImageProducer);
        var sut = new UploadFileHandler(fileUploadCommandFactory, metadataUploadCommandFactory,
            publishNewMessageCommandFactory);

        var image = await sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.ReceivedWithAnyArgs().WriteMetadataAsync(default!);
        await newImageProducer.ReceivedWithAnyArgs().SendAsync(default!);
        image.Id.Should().Be(imageId);
        image.UserId.Should().Be(userId);
        image.ObjectName.Should().Be(formFile.FileName);
        image.Hidden.Should().Be(hidden);
        image.UploadedAt.Should().Be(uploadedAt);
    }

    [Fact]
    public async Task Upload_already_exists_file()
    {
        var imageId = new ImageId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var metadataService = Substitute.For<IMetadataService>();
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var fileService = Substitute.For<IFileService>();
        fileService.WriteFileAsync(Arg.Any<WriteFileDto>())
            .ThrowsAsync(new ImageObjectAlreadyExistsException(userId.ToString(), formFile.FileName));
        var newImageProducer = Substitute.For<INewImageProducer>();
        var publishNewMessageCommandFactory = new PublishNewMessageCommandFactory(newImageProducer);
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var sut = new UploadFileHandler(fileUploadCommandFactory, metadataUploadCommandFactory,
            publishNewMessageCommandFactory);

        var act = () => sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await act.Should().ThrowAsync<RollbackCommandsException>().WithInnerException(typeof(ImageObjectAlreadyExistsException));
        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.DidNotReceiveWithAnyArgs().WriteMetadataAsync(default!);
        await newImageProducer.DidNotReceiveWithAnyArgs().SendAsync(default!);
    }

    [Fact]
    public async Task Write_already_exists_metadata()
    {
        var imageId = new ImageId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var metadataService = Substitute.For<IMetadataService>();
        metadataService.WriteMetadataAsync(Arg.Any<ImageMetadataDto>())
            .ThrowsAsync<DbUpdateException>();
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var fileService = Substitute.For<IFileService>();
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var newImageProducer = Substitute.For<INewImageProducer>();
        var publishNewMessageCommandFactory = new PublishNewMessageCommandFactory(newImageProducer);
        var sut = new UploadFileHandler(fileUploadCommandFactory, metadataUploadCommandFactory,
            publishNewMessageCommandFactory);

        var act = () => sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await act.Should().ThrowAsync<RollbackCommandsException>().WithInnerException(typeof(DbUpdateException));
        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.ReceivedWithAnyArgs().WriteMetadataAsync(default!);
        await fileService.ReceivedWithAnyArgs().RemoveFileAsync(default!);
        await newImageProducer.DidNotReceiveWithAnyArgs().SendAsync(default!);
    }

    [Fact]
    public async Task Could_not_send_event()
    {
        var imageId = new ImageId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());
        var formFile = FileFixture.GetPlainTextFormFile();
        const bool hidden = false;
        var uploadedAt = DateTime.Now;
        var metadataService = Substitute.For<IMetadataService>();
        var metadataUploadCommandFactory = new MetadataUploadCommandFactory(metadataService);
        var fileService = Substitute.For<IFileService>();
        var fileUploadCommandFactory = new FileUploadCommandFactory(fileService);
        var newImageProducer = Substitute.For<INewImageProducer>();
        newImageProducer.SendAsync(Arg.Any<NewImage>())
            .ThrowsAsync(new KafkaException(new Error(ErrorCode.NetworkException)));
        var publishNewMessageCommandFactory = new PublishNewMessageCommandFactory(newImageProducer);
        var sut = new UploadFileHandler(fileUploadCommandFactory, metadataUploadCommandFactory,
            publishNewMessageCommandFactory);

        var act = () => sut.UploadAsync(userId, imageId, formFile, hidden, uploadedAt);

        await act.Should().ThrowAsync<RollbackCommandsException>().WithInnerException(typeof(KafkaException));
        await fileService.ReceivedWithAnyArgs().WriteFileAsync(default!);
        await metadataService.ReceivedWithAnyArgs().WriteMetadataAsync(default!);
        await newImageProducer.ReceivedWithAnyArgs().SendAsync(default!);
        await metadataService.ReceivedWithAnyArgs().DeleteMetadataAsync(default!);
        await fileService.ReceivedWithAnyArgs().RemoveFileAsync(default!);
    }
}
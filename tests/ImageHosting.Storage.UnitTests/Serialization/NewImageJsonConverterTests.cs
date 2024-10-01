namespace ImageHosting.Storage.UnitTests.Serialization;

using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.Serialization;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.UnitTests.Extensions;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

public class NewImageJsonConverterTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public NewImageJsonConverterTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new NewImageJsonConverter() },
            WriteIndented = false
        };
    }

    [Fact]
    public void Serialize_NewImage_returns_correct_JSON()
    {
        var newImage = new NewImage
        {
            Bucket = new UserId(Guid.Empty),
            ImageId = new ImageId(Guid.Empty),
            Name = "image.jpg",
        };

        var json = JsonSerializer.Serialize(newImage, _jsonSerializerOptions);
        
        _testOutputHelper.WriteLine("JSON string: {0}", json);
        json.Should().Contain("\"bucket\":\"00000000-0000-0000-0000-000000000000\"");
        json.Should().Contain("\"prefix\":\"00000000-0000-0000-0000-000000000000\"");
        json.Should().Contain("\"name\":\"image.jpg\"");
        json.Should().NotContain("imageId");  // Ensure ImageId is ignored
    }

    [Fact]
    public void Deserialize_valid_JSON_returns_NewImage()
    {
        var json = @"{
            ""bucket"": ""00000000-0000-0000-0000-000000000000"",
            ""prefix"": ""00000000-0000-0000-0000-000000000000"",
            ""name"": ""image.jpg""
        }";

        var newImage = JsonSerializer.Deserialize<NewImage>(json, _jsonSerializerOptions);

        _testOutputHelper.PrintProperties(newImage);
        Assert.NotNull(newImage);
        Assert.Equal(new UserId(Guid.Empty), newImage.Bucket);
        Assert.Equal("image.jpg", newImage.Name);
        Assert.Equal(new ImageId(Guid.Empty), newImage.ImageId);
    }

    [Fact]
    public void Deserialize_invalid_JSON_throws_JSON_exception()
    {
        // Missing required prefix field
        var invalidJson = @"{
            ""bucket"": ""00000000-0000-0000-0000-000000000000"",
            ""image"": ""image.jpg""
        }";

        _testOutputHelper.WriteLine("JSON string: {0}", invalidJson);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<NewImage>(invalidJson, _jsonSerializerOptions));
    }
}

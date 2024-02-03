using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.UnitTests.Xunit2;

public static class FileFixture
{
    public static FormFile GetPlainTextFormFile()
    {
        var buffer = "Lorem ipsum dolor sit amet"u8.ToArray();
        var formFile = new FormFile(new MemoryStream(buffer), 0, buffer.Length, "test.txt", "test.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
        return formFile;
    }
}

namespace ImageHosting.Storage.Features.Images.Models;

public class ReadImageDto
{
    public ReadImageDto(string url)
    {
        Url = url;
    }

    public string Url { get; set; }
}

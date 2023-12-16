namespace ImageHosting.Storage.Features.Images;

public class ReadImageDto
{
    public ReadImageDto(string url)
    {
        Url = url;
    }

    public string Url { get; set; }
}

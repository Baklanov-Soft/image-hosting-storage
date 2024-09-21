using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.WebApi.Options;

public class ImagesOptions
{
    public const string SectionName = "Images";

    [Required] public required Uri BaseUri { get; init; }
}
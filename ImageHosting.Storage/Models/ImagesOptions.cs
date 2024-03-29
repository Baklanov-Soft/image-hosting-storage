using System;
using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Models;

public class ImagesOptions
{
    public const string SectionName = "Images";

    [Required(AllowEmptyStrings = false)] public required Uri BaseUri { get; init; }
}
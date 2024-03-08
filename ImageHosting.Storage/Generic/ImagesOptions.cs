using System;
using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Generic;

public class ImagesOptions
{
    public const string SectionName = "Images";

    [Required(AllowEmptyStrings = false)] public required Uri BaseUri { get; init; }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Generic;

public class MinioOptions
{
    public const string SectionName = "Minio";

    [Required] public required Uri Endpoint { get; init; }
    [Required(AllowEmptyStrings = false)] public required string AccessKey { get; init; }
    [Required(AllowEmptyStrings = false)] public required string SecretKey { get; init; }
    public bool Secure { get; init; }
}
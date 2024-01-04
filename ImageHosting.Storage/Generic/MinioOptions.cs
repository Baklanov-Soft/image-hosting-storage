using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Generic;

public class MinioOptions
{
    public const string SectionName = "Minio";

    [Required(AllowEmptyStrings = false)] public string Endpoint { get; init; } = null!;
    [Required(AllowEmptyStrings = false)] public string AccessKey { get; init; } = null!;
    [Required(AllowEmptyStrings = false)] public string SecretKey { get; init; } = null!;
    public bool Secure { get; init; }
}

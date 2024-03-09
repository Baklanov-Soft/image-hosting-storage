using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Models;

public class MinioOptions
{
    public const string SectionName = "Minio";

    [Required(AllowEmptyStrings = false)] public required string Endpoint { get; init; }
    [Required(AllowEmptyStrings = false)] public required string AccessKey { get; init; }
    [Required(AllowEmptyStrings = false)] public required string SecretKey { get; init; }
    public bool Secure { get; init; }
}
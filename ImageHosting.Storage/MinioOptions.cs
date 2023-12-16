using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage;

public class MinioOptions
{
    public const string SectionName = "Minio";

    [Required(AllowEmptyStrings = false)] public string Endpoint { get; set; } = null!;
    [Required(AllowEmptyStrings = false)] public string AccessKey { get; set; } = null!;
    [Required(AllowEmptyStrings = false)] public string SecretKey { get; set; } = null!;
    public bool Secure { get; set; }
}

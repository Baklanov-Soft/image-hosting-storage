using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Infrastructure.Options;

public class AssignTagsOptions
{
    public const string SectionName = "AssignTags";

    [Required] public required double Threshold { get; init; }
}
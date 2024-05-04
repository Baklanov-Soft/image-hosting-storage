using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Tagger.Options;

public class AssignTagsConsumerOptions
{
    public const string SectionName = "AssignTagsConsumer";
    
    [Required] public required string[] BootstrapServers { get; init; }
    [Required] public required string TopicName { get; init; }
    [Required] public required string GroupId { get; init; }
}
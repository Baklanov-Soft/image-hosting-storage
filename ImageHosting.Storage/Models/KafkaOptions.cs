using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace ImageHosting.Storage.Models;

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    [Required] public required AdminClientConfig Admin { get; init; }
    [Required] public required ProducerConfig Producer { get; init; }
    [Required] public required ConsumerConfig Consumer { get; init; }
    [Required] public required TopicSpecification NewImagesTopic { get; init; }
    [Required] public required TopicOptions CategoriesTopic { get; init; }
}

public class TopicOptions
{
    [Required(AllowEmptyStrings = false)] public required string Name { get; init; }
    [Required] public required double Threshold { get; init; }
    [Required] public required long CommitPeriod { get; init; }
}
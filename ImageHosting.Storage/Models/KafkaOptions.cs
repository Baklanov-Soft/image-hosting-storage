using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace ImageHosting.Storage.Models;

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    [Required] public required AdminClientConfig Admin { get; init; }
    [Required] public required ProducerConfig Producer { get; init; }
    [Required] public required TopicSpecification NewImagesTopic { get; init; }
}
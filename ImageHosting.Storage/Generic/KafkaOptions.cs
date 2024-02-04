using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;

namespace ImageHosting.Storage.Generic;

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    [Required] public required ProducerConfig Producer { get; init; }
    [Required(AllowEmptyStrings = false)] public required string NewImageTopic { get; init; }
}
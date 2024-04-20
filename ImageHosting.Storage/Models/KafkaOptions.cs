using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Storage.Models;

public class KafkaOptions
{
    public const string SectionName = "Kafka";

    [Required] public required string[] BootstrapServers { get; init; }
    [Required] public required TopicOptions NewImagesProducer { get; init; }
    [Required] public required CategoriesTopicConsumerOptions CategoriesConsumer { get; init; }
}

public class TopicOptions
{
    [Required(AllowEmptyStrings = false)] public required string TopicName { get; init; }
}

public class CategoriesTopicConsumerOptions : TopicOptions
{
    [Required(AllowEmptyStrings = false)] public required string GroupId { get; init; }
    [Required] public required double Threshold { get; init; }
}
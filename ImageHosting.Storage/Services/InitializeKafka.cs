using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using ImageHosting.Storage.Models;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Services;

public class InitializeKafka(IOptions<KafkaOptions> options) : IInitializeKafka
{
    private readonly IAdminClient _adminClient = new AdminClientBuilder(options.Value.Admin).Build();
    private readonly TopicSpecification _newImageTopicSpecification = options.Value.NewImageTopic;

    public async Task CreateNewImageTopic()
    {
        try
        {
            _ = await _adminClient.DescribeTopicsAsync(
                TopicCollection.OfTopicNames([_newImageTopicSpecification.Name]));
        }
        catch (DescribeTopicsException)
        {
            await _adminClient.CreateTopicsAsync([_newImageTopicSpecification]);
        }
    }
}
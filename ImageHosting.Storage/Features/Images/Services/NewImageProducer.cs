using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Services;

public sealed class NewImageProducer : INewImageProducer
{
    private readonly IProducer<Null, NewImage> _producer;
    private readonly KafkaOptions _options;

    public NewImageProducer(IOptions<KafkaOptions> options)
    {
        _options = options.Value;
        _producer = new ProducerBuilder<Null, NewImage>(_options.Producer)
            .SetValueSerializer(new NewImage.Serializer())
            .Build();
    }

    public Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        return _producer.ProduceAsync(_options.NewImageTopic, new Message<Null, NewImage>
        {
            Value = newImage
        }, cancellationToken);
    }

    public void Dispose() => _producer.Dispose();
}
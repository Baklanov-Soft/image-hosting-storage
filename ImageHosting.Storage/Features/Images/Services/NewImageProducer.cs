using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Models;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage.Features.Images.Services;

public sealed class NewImageProducer(IOptions<KafkaOptions> options) : INewImageProducer
{
    private readonly IProducer<Null, NewImage> _producer =
        new ProducerBuilder<Null, NewImage>(options.Value.Producer)
            .SetValueSerializer(new NewImageSerializer())
            .Build();

    public Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default)
    {
        return _producer.ProduceAsync(options.Value.NewImagesTopic.Name, new Message<Null, NewImage>
        {
            Value = newImage
        }, cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}
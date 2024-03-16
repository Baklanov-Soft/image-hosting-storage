using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage;

public sealed class ImageTaggingWorker(IOptions<KafkaOptions> options, ILogger<ImageTaggingWorker> logger) : BackgroundService
{
    private readonly IConsumer<Null, CategorizedNewImage> _consumer =
        new ConsumerBuilder<Null, CategorizedNewImage>(options.Value.Consumer)
            .SetValueDeserializer(new CategorizedNewImageDeserializer())
            .Build();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
    }

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(options.Value.CategoriesTopic.Name);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                // Handle message...

                _consumer.StoreOffset(consumeResult);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                // Consumer errors should generally be ignored (or logged) unless fatal.

                if (e.Error.IsFatal)
                {
                    // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                    break;
                }
            }
            catch (Exception)
            {
                break;
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close(); // Commit offsets and leave the group cleanly.
        _consumer.Dispose();

        base.Dispose();
    }
}
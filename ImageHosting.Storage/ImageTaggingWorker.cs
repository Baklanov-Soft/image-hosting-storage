using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Features.Images.Services;
using ImageHosting.Storage.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ImageHosting.Storage;

public sealed class ImageTaggingWorker(IOptions<KafkaOptions> options, IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly IConsumer<Null, CategorizedNewImage> _consumer =
        new ConsumerBuilder<Null, CategorizedNewImage>(options.Value.Consumer)
            .SetValueDeserializer(new CategorizedNewImageDeserializer())
            .Build();

    private readonly string _categoriesTopicName = options.Value.CategoriesTopic.Name;
    private readonly double _threshold = options.Value.CategoriesTopic.Threshold;
    private readonly long _commitPeriod = options.Value.CategoriesTopic.CommitPeriod;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_categoriesTopicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                using var scope = serviceScopeFactory.CreateScope();
                var assignCategoriesService = scope.ServiceProvider.GetRequiredService<IAssignTagsService>();

                var categorizedNewImage = consumeResult.Message.Value;

                await assignCategoriesService.AssignTagsAsync(categorizedNewImage.ImageId,
                    categorizedNewImage.Categories, _threshold, stoppingToken);

                _consumer.StoreOffset(consumeResult);
                if (consumeResult.Offset % _commitPeriod == 0)
                {
                    try
                    {
                        _consumer.Commit(consumeResult);
                    }
                    catch (KafkaException)
                    {
                        break;
                    }
                }
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
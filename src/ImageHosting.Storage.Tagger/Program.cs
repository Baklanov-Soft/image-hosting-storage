using CommunityToolkit.Diagnostics;
using Confluent.Kafka;
using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.Serialization;
using ImageHosting.Storage.Infrastructure.Extensions.DependencyInjection;
using ImageHosting.Storage.Tagger;
using ImageHosting.Storage.Tagger.Options;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

var options = builder.Configuration
    .GetSection(AssignTagsConsumerOptions.SectionName)
    .Get<AssignTagsConsumerOptions>();
Guard.IsNotNull(options);

builder.Services.AddMassTransit(massTransit =>
{
    massTransit.UsingInMemory((context, configurator) =>
    {
        configurator.ConfigureJsonSerializerOptions(jsonSerializerOptions =>
        {
            jsonSerializerOptions.Converters.Add(new NewImageJsonConverter());
            return jsonSerializerOptions;
        });
    });

    massTransit.AddRider(rider =>
    {
        rider.AddConsumer<AssignTagsConsumer>(consumerDefinitionType: typeof(AssignTagsConsumerDefinition));

        rider.UsingKafka((context, kafka) =>
        {
            kafka.Host(options.BootstrapServers);
            kafka.TopicEndpoint<CategorizedNewImage>(options.TopicName, options.GroupId, endpoint =>
            {
                endpoint.ConfigureConsumer<AssignTagsConsumer>(context);
                endpoint.AutoOffsetReset = AutoOffsetReset.Earliest;
                endpoint.ConcurrentDeliveryLimit = 10;
            });
        });
    });
});
builder.Services.AddImageHostingDbContext("ImageHosting");
builder.Services.AddAssignTagsService();

var host = builder.Build();
host.Run();
using MassTransit;

namespace ImageHosting.Storage.Tagger;

public class AssignTagsConsumerDefinition : ConsumerDefinition<AssignTagsConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<AssignTagsConsumer> consumerConfigurator, IRegistrationContext context)
    {
        consumerConfigurator.Options<BatchOptions>(options => options
            .SetMessageLimit(10)
            .SetTimeLimit(TimeSpan.FromSeconds(1)));
    } }
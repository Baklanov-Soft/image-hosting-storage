using System;
using MassTransit;

namespace ImageHosting.Storage;

public class AssignTagsConsumerDefinition : ConsumerDefinition<AssignTagsConsumer>
{
    public AssignTagsConsumerDefinition()
    {
        Endpoint(endpointRegistration =>
        {
            endpointRegistration.PrefetchCount = 10;
        });
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<AssignTagsConsumer> consumerConfigurator, IRegistrationContext context)
    {
        consumerConfigurator.Options<BatchOptions>(options => options
            .SetMessageLimit(10)
            .SetTimeLimit(TimeSpan.FromSeconds(3)));
    }
}
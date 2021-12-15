using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.RabbitMQ;

public static class RabbitMQEventBusBuilderExtensions
{
    public static EventBusBuilder WithRabbitMQ(this EventBusBuilder builder, Action<RabbitMQOptions> configureOptions)
    {
        builder.Services.Configure(configureOptions);
        builder.Services.TryAddSingleton<IEventBus, EventBus>();

        return builder;
    }
}

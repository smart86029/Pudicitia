using Microsoft.Extensions.DependencyInjection;

namespace Pudicitia.Common.Events;

public sealed class EventBusBuilder
{
    internal EventBusBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; private init; }
}

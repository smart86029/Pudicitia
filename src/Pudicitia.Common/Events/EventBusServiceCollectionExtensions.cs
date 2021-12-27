using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Pudicitia.Common.Events;

namespace Microsoft.Extensions.DependencyInjection;

public static class EventBusServiceCollectionExtensions
{
    public static EventBusBuilder AddEventBus(this IServiceCollection services)
    {
        services.AddHostedService<EventBusBackgroundService>();

        var assemblies = DependencyContext.Default
            .GetDefaultAssemblyNames()
            .Where(x => x.Name!.StartsWith("Pudicitia."))
            .Where(x => x.Name!.EndsWith(".Events"))
            .Select(x => Assembly.Load(x));
        var eventHandlerTypes = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsAssignableToGenericType(typeof(IEventHandler<>)))
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract);
        foreach (var eventHandlerType in eventHandlerTypes)
        {
            var interfaceTypes = eventHandlerType
                .GetInterfaces()
                .Where(x => x.IsAssignableToGenericType(typeof(IEventHandler<>)));
            foreach (var interfaceType in interfaceTypes)
            {
                services.TryAddTransient(interfaceType, eventHandlerType);
            }
        }

        return new EventBusBuilder(services);
    }
}

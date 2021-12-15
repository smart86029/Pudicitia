using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Pudicitia.Common.Events;

public static class EventBusApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEventBus(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
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
            var interfaces = eventHandlerType
                .GetInterfaces()
                .Where(x => x.IsAssignableToGenericType(typeof(IEventHandler<>)));
            foreach (var @interface in interfaces)
            {
                eventBus.Subscribe(@interface.GenericTypeArguments[0], @interface);
            }
        }

        return app;
    }
}

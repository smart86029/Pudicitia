using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.Events
{
    public static class EventBusServiceCollectionExtensions
    {
        public static EventBusBuilder AddEventBus(this IServiceCollection services)
        {
            services.AddHostedService<EventBusBackgroundService>();

            var assemblies = DependencyContext.Default
                .GetDefaultAssemblyNames()
                .Where(x => x.Name.StartsWith("Pudicitia."))
                .Where(x => x.Name.EndsWith(".Events"))
                .Select(x => Assembly.Load(x));
            var eventHandlerTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsAssignableToGenericType(typeof(IEventHandler<>)))
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract);
            foreach (var eventHandlerType in eventHandlerTypes)
            {
                var serviceTypes = eventHandlerType
                    .GetInterfaces()
                    .Where(x => x.IsAssignableToGenericType(typeof(IEventHandler<>)));
                foreach (var serviceType in serviceTypes)
                    services.TryAddTransient(serviceType, eventHandlerType);
            }

            return new EventBusBuilder(services);
        }
    }
}
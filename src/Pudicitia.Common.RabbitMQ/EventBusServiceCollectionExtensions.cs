using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.RabbitMQ
{
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, Action<RabbitMQOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.TryAddSingleton<IEventBus, EventBus>();

            var assemblies = DependencyContext
                .Default
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
                foreach (var @interface in serviceTypes)
                    services.TryAddTransient(@interface, eventHandlerType);
            }

            return services;
        }
    }
}
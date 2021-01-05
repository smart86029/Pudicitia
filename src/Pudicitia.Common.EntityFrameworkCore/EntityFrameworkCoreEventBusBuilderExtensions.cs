using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore
{
    public static class EntityFrameworkCoreEventBusBuilderExtensions
    {
        public static EventBusBuilder WithEntityFrameworkCore<TContext>(this EventBusBuilder builder)
            where TContext : DbContext
        {
            builder.Services.TryAddScoped<IEventPublishedRepository, EventPublishedRepository<TContext>>();
            builder.Services.TryAddScoped<IEventSubscribedRepository, EventSubscribedRepository<TContext>>();

            return builder;
        }
    }
}
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;

namespace Microsoft.Extensions.DependencyInjection;

public static class JaegerServiceCollectionExtensions
{
    public static IServiceCollection AddJaeger(this IServiceCollection services)
    {
        services.AddOpenTracing();
        services.AddSingleton(serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                .RegisterSenderFactory<ThriftSenderFactory>();
            var tracer = Jaeger.Configuration
                .FromEnv(loggerFactory)
                .GetTracer();

            GlobalTracer.Register(tracer);

            return tracer;
        });

        return services;
    }
}

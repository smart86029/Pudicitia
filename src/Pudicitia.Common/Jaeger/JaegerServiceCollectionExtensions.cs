using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Pudicitia.Common.Jaeger;

namespace Microsoft.Extensions.DependencyInjection;

public static class JaegerServiceCollectionExtensions
{
    public static IServiceCollection AddJaeger(this IServiceCollection services, JaegerOptions? jaegerOptions)
    {
        if (jaegerOptions is null)
        {
            return services;
        }

        services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(jaegerOptions.ServiceName))
                .AddSource(jaegerOptions.ServiceName)
                .AddJaegerExporter(jaegerExporterOptions =>
                {
                    jaegerExporterOptions.AgentHost = jaegerOptions.AgentHost;
                    jaegerExporterOptions.AgentPort = jaegerOptions.AgentPort;
                })
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation();
        });

        return services;
    }
}

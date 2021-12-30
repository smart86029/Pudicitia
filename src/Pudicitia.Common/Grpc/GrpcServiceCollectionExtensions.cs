using Microsoft.Extensions.DependencyInjection.Extensions;
using Pudicitia.Common.Grpc;

namespace Microsoft.Extensions.DependencyInjection;

public static class GrpcServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClient<TClient>(this IServiceCollection services, Uri address)
         where TClient : class
    {
        services.TryAddTransient<LoggingInterceptor>();
        services.TryAddTransient<BypassHttpClientHandler>();

        services
            .AddGrpcClient<TClient>(x => x.Address = address)
            .AddInterceptor<LoggingInterceptor>()
            .ConfigurePrimaryHttpMessageHandler<BypassHttpClientHandler>();

        return services;
    }
}

using Pudicitia.Common.Dapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class DapperServiceCollectionExtensions
{
    public static IServiceCollection AddDapper(this IServiceCollection services, Action<DapperOptions> configureOptions)
    {
        services.Configure(configureOptions);

        SqlMapper.AddTypeHandler(new DateTimeHandler());

        return services;
    }
}

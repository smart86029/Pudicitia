using Microsoft.Extensions.DependencyModel;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppServiceCollectionExtensions
{
    public static IServiceCollection AddApps(this IServiceCollection services)
    {
        var assemblies = DependencyContext.Default
            .GetDefaultAssemblyNames()
            .Where(x => x.Name!.StartsWith("Pudicitia."))
            .Where(x => x.Name!.EndsWith(".App"))
            .Select(x => Assembly.Load(x));
        var appTypes = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x.Name.EndsWith("App"));
        foreach (var appType in appTypes)
        {
            services.AddScoped(appType);
        }

        return services;
    }
}

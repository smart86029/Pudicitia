using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Pudicitia.Common.Domain;

public static class DomainServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var assemblies = DependencyContext.Default
            .GetDefaultAssemblyNames()
            .Where(x => x.Name!.StartsWith("Pudicitia."))
            .Where(x => x.Name!.EndsWith(".Data"))
            .Select(x => Assembly.Load(x));
        var repositoryTypes = assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsAssignableToGenericType(typeof(IRepository<>)));
        foreach (var repositoryType in repositoryTypes)
        {
            foreach (var interfaceType in repositoryType.GetInterfaces())
            {
                if (interfaceType != typeof(IRepository<>))
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }
        }

        return services;
    }
}

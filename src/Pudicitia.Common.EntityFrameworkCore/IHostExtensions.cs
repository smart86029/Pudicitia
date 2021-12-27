using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

public static class IHostExtensions
{
    public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetService<TContext>();

        try
        {
            context.Database.Migrate();
            seeder.Invoke(context);
        }
        catch
        {
        }

        return host;
    }
}

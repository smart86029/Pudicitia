using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pudicitia.Identity.Api.Extensions
{
    public static class IHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext> seeder) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetService<TContext>();
            var env = services.GetService<IWebHostEnvironment>();

            try
            {
                context.Database.Migrate();
                if (env.IsDevelopment())
                    seeder(context);
            }
            catch
            {
            }

            return host;
        }
    }
}
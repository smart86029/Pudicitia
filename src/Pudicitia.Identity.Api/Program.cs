using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pudicitia.Identity.Api.Extensions;
using Pudicitia.Identity.Data;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace Pudicitia.Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Information()
               .WriteTo.Console()
               .WriteTo.GrafanaLoki("http://loki:3100")
               .CreateLogger();

            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<IdentityContext>(context => new IdentityContextSeed(context).SeedAsync().Wait())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
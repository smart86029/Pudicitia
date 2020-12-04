using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pudicitia.HR.Api.Extensions;
using Pudicitia.HR.Data;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace Pudicitia.HR.Api
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
                .MigrateDbContext<HRContext>(context => new HRContextSeed(context).SeedAsync().Wait())
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
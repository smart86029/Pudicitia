using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pudicitia.Identity.Api.Extensions;
using Pudicitia.Identity.Data;
using Serilog;
using Serilog.Events;

namespace Pudicitia.Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Information()
               .WriteTo.Async(x => x.Console(LogEventLevel.Debug))
               .CreateLogger();

            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<IdentityContext>(context => new IdentityContextSeed(context).SeedAsync().Wait())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
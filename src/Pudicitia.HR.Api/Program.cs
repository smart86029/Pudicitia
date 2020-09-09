using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pudicitia.HR.Api.Extensions;
using Pudicitia.HR.Data;

namespace Pudicitia.HR.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<HRContext>(context => new HRContextSeed(context).SeedAsync().Wait())
                .Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
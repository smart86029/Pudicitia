using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace Pudicitia.Common.Serilog;

public static class SerilogFactory
{
    public static ILogger CreateLogger(IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.GrafanaLoki(configuration["Serilog:Loki"])
           .CreateLogger();

        return logger;
    }
}

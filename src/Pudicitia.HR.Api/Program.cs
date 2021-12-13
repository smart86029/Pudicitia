using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.EntityFrameworkCore;
using OpenTracing.Util;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.RabbitMQ;
using Pudicitia.HR.Api;
using Pudicitia.HR.Data;
using Pudicitia.HR.Domain;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddGrpc();
services.AddDbContext<HRContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

var assemblyApp = Assembly.Load("Pudicitia.HR.App");
var appTypes = assemblyApp
    .GetTypes()
    .Where(x => x.Name.EndsWith("App"));
foreach (var appType in appTypes)
    services.AddScoped(appType);

var assemblyData = Assembly.Load("Pudicitia.HR.Data");
var repositoryTypes = assemblyData
    .GetTypes()
    .Where(x => x.IsAssignableToGenericType(typeof(IRepository<>)));
foreach (var repositoryType in repositoryTypes)
{
    foreach (var interfaceType in repositoryType.GetInterfaces())
        if (interfaceType != typeof(IRepository<>))
            services.AddScoped(interfaceType, repositoryType);
}

services
    .AddEventBus()
    .WithEntityFrameworkCore<HRContext>()
    .WithRabbitMQ(options =>
    {
        options.ConnectionString = configuration.GetConnectionString("RabbitMQ");
        options.QueueName = "hr";
    });

services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();
services.AddOpenTracing();
services.AddSingleton(serviceProvider =>
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver =
        new SenderResolver(loggerFactory).RegisterSenderFactory<ThriftSenderFactory>();
    var tracer = Jaeger.Configuration
        .FromEnv(loggerFactory)
        .GetTracer();

    GlobalTracer.Register(tracer);

    return tracer;
});

Log.Logger = new LoggerConfiguration()
   .Enrich.FromLogContext()
   .WriteTo.Console()
   .WriteTo.GrafanaLoki("http://loki:3100")
   .CreateLogger();
builder.WebHost.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrganizationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MigrateDbContext<HRContext>(context => new HRContextSeed(context).SeedAsync().Wait());
app.Run();

using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.EntityFrameworkCore;
using OpenTracing.Util;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.RabbitMQ;
using Pudicitia.Identity.Api;
using Pudicitia.Identity.Data;
using Pudicitia.Identity.Domain;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
builder.Services.AddRazorPages();

services.AddGrpc();

services.AddControllersWithViews();

services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

var assemblyApp = Assembly.Load("Pudicitia.Identity.App");
var appTypes = assemblyApp
    .GetTypes()
    .Where(x => x.Name.EndsWith("App"));
foreach (var appType in appTypes)
    services.AddScoped(appType);

var assemblyData = Assembly.Load("Pudicitia.Identity.Data");
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
    .WithEntityFrameworkCore<IdentityContext>()
    .WithRabbitMQ(options =>
    {
        options.ConnectionString = configuration.GetConnectionString("RabbitMQ");
        options.QueueName = "identity";
    });

services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();

services
    .AddIdentityServer(options =>
    {
        options.UserInteraction.LoginUrl = "/Authentication/SignIn";
        options.UserInteraction.LogoutUrl = "/Authentication/SignOut";
        options.UserInteraction.LogoutIdParameter = "signOutId";
    })
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MigrateDbContext<IdentityContext>(context => new IdentityContextSeed(context).SeedAsync().Wait());
app.Run();

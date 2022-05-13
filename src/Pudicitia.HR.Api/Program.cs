using Microsoft.EntityFrameworkCore;
using Prometheus;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Serilog;
using Pudicitia.HR.Api;
using Pudicitia.HR.Data;
using Pudicitia.HR.Domain;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    Log.Logger = SerilogFactory.CreateLogger(configuration);

    services.AddGrpc();
    services.AddDbContext<HRContext>(options => options.UseSqlServer(configuration.GetConnectionString("Database")));

    services.AddApps();
    services.AddRepositories();
    services.AddScoped<IHRUnitOfWork, HRUnitOfWork>();
    services.AddJaeger();

    services
        .AddEventBus()
        .WithEntityFrameworkCore<HRContext>()
        .WithRabbitMQ(options =>
        {
            options.Uri = new Uri(configuration.GetConnectionString("EventBus"));
            options.ClientName = "Pudicitia.HR.Api";
        });

    Log.Information("Services were configured.");

    builder.Host.UseSerilog();

    var app = builder.Build();

    app.UseEventBus();

    app.MapMetrics();
    app.MapGrpcService<OrganizationService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    Log.Information("Middlewares were added.");

    app.MigrateDbContext<HRContext>(context => new HRContextSeed(context).SeedAsync().Wait());
    Log.Information("Context was migrated.");

    app.Run();
    Log.Information($"Application has started.");

    return 0;
}
catch (Exception exception)
{
    Log.Fatal(exception, "Application terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

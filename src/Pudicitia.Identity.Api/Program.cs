using Microsoft.EntityFrameworkCore;
using Prometheus;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Serilog;
using Pudicitia.Identity.Api;
using Pudicitia.Identity.Data;
using Pudicitia.Identity.Domain;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    Log.Logger = SerilogFactory.CreateLogger(configuration);

    services.AddRazorPages();
    services.AddGrpc();
    services.AddSqlServer<IdentityContext>(configuration.GetConnectionString("Database"));

    services.AddApps();
    services.AddRepositories();
    services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
    services.AddJaeger();

    services
        .AddEventBus()
        .WithEntityFrameworkCore<IdentityContext>()
        .WithRabbitMQ(options =>
        {
            options.Uri = new Uri(configuration.GetConnectionString("EventBus"));
            options.ClientName = "Pudicitia.Identity.Api";
        });

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

    Log.Information("Services were configured.");

    builder.WebHost.UseSerilog();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseGrpcMetrics();
    app.UseEventBus();

    app.UseAuthorization();

    app.UseIdentityServer();

    app.MapGrpcService<AuthorizationService>();
    app.MapMetrics();
    app.MapRazorPages();
    Log.Information("Middlewares were added.");

    app.MigrateDbContext<IdentityContext>(context => new IdentityContextSeed(context).SeedAsync().Wait());
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

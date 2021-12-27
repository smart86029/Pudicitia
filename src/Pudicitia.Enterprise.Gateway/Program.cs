using Pudicitia.Common.Grpc;
using Pudicitia.Common.Serilog;
using Pudicitia.Enterprise.Gateway;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    Log.Logger = SerilogFactory.CreateLogger(configuration);

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddJaeger();

    services
        .AddGrpcClient<Authorization.AuthorizationClient>(x => x.Address = new Uri(configuration["Apis:Identity"]))
        .AddInterceptor<LoggingInterceptor>();

    services
        .AddGrpcClient<Organization.OrganizationClient>(x => x.Address = new Uri(configuration["Apis:HR"]))
        .AddInterceptor<LoggingInterceptor>();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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

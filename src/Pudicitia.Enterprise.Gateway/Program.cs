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

    var addressIdentity = new Uri(configuration["Apis:Identity"]);
    var addressHR = new Uri(configuration["Apis:HR"]);
    services
        .AddGrpcClient<Authorization.AuthorizationClient>(addressIdentity)
        .AddGrpcClient<Organization.OrganizationClient>(addressHR);

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

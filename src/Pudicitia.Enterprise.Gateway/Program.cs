using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Pudicitia.Common.Identity;
using Pudicitia.Common.Serilog;
using Pudicitia.Enterprise.Gateway;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    Log.Logger = SerilogFactory.CreateLogger(configuration);

    var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>();
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = jwtConfig.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtConfig.ValidIssuer,
                ValidateAudience = false,
            };

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
            };
            options.Backchannel = new HttpClient(handler);
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("HumanResources", policy => policy.RequireClaim(IdentityClaimTypes.Permission, "HumanResources"));
    });

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddJaeger();

    var addressIdentity = new Uri(configuration["Apis:Identity"]);
    var addressHR = new Uri(configuration["Apis:HR"]);
    services
        .AddGrpcClient<Authorization.AuthorizationClient>(addressIdentity)
        .AddGrpcClient<Attendance.AttendanceClient>(addressHR)
        .AddGrpcClient<Organization.OrganizationClient>(addressHR);

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseHttpMetrics();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapMetrics();
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

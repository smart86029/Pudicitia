using System.Linq;
using System.Reflection;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.RabbitMQ;
using Pudicitia.HR.Data;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();

            services.AddDbContext<HRContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));

            var assemblyApp = Assembly.Load("Pudicitia.HR.App");
            var apps = assemblyApp
                .GetTypes()
                .Where(x => x.Name.EndsWith("App"));
            foreach (var app in apps)
                services.AddScoped(app);

            var assemblyData = Assembly.Load("Pudicitia.HR.Data");
            var repositories = assemblyData
                .GetTypes()
                .Where(x => x.IsAssignableToGenericType(typeof(IRepository<>)));
            foreach (var repository in repositories)
            {
                foreach (var @interface in repository.GetInterfaces())
                    if (@interface != typeof(IRepository<>))
                        services.AddScoped(@interface, repository);
            }

            services.AddEventBus(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("RabbitMQ");
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrganizationService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            app.UseEventBus();
        }
    }
}
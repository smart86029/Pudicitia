using System.Linq;
using System.Reflection;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using Prometheus;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.RabbitMQ;
using Pudicitia.Identity.Data;
using Pudicitia.Identity.Domain;

namespace Pudicitia.Identity.Api
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

            services.AddControllersWithViews();

            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));

            var assemblyApp = Assembly.Load("Pudicitia.Identity.App");
            var apps = assemblyApp
                .GetTypes()
                .Where(x => x.Name.EndsWith("App"));
            foreach (var app in apps)
                services.AddScoped(app);

            var assemblyData = Assembly.Load("Pudicitia.Identity.Data");
            var repositories = assemblyData
                .GetTypes()
                .Where(x => x.IsAssignableToGenericType(typeof(IRepository<>)));
            foreach (var repository in repositories)
            {
                foreach (var @interface in repository.GetInterfaces())
                    if (@interface != typeof(IRepository<>))
                        services.AddScoped(@interface, repository);
            }

            services
                .AddEventBus()
                .WithEntityFrameworkCore<IdentityContext>()
                .WithRabbitMQ(options =>
                {
                    options.ConnectionString = Configuration.GetConnectionString("RabbitMQ");
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseGrpcMetrics();

            app.UseAuthorization();

            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuthorizationService>();
                endpoints.MapMetrics();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEventBus();
        }
    }
}
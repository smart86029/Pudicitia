using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pudicitia.Common.Domain;
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

            services.AddEventBus(options =>
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<AuthorizationService>();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEventBus();
        }
    }
}
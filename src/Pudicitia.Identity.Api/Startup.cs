using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Extensions;
using Pudicitia.Identity.App.Account;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSingleton(new JwtSettings(
                Configuration["Jwt:Key"],
                Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                TimeSpan.FromSeconds(Configuration["Jwt:AccessTokenExpiry"].ToInt()),
                TimeSpan.FromSeconds(Configuration["Jwt:RefreshTokenExpiry"].ToInt())));

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

            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();

            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
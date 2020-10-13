using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pudicitia.Enterprise.Gateway
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
            services.AddControllers();

            services.AddTransient<A>();

            services
                .AddGrpcClient<Authorization.AuthorizationClient>(x => x.Address = new Uri(Configuration["Apis:Identity"]))
                .AddInterceptor<A>()
                .ConfigurePrimaryHttpMessageHandler(() => GetClientHandler());

            services
                .AddGrpcClient<Organization.OrganizationClient>(x => x.Address = new Uri(Configuration["Apis:HR"]))
                .AddInterceptor<A>()
                .ConfigurePrimaryHttpMessageHandler(() => GetClientHandler());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private HttpClientHandler GetClientHandler()
        {
            var result = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            };

            return result;
        }
    }

    public class A : Interceptor
    {
        private ILogger<A> logger;

        public A(ILogger<A> logger)
        {
            this.logger = logger;
        }


        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                //return base.AsyncUnaryCall(request, context, continuation);
                return continuation(request, context);
            }
            catch (Exception e)
            {
                logger.LogError(e, "123");
                throw new RpcException(new Status(StatusCode.Unknown, e.ToString()));
            }
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {

            try
            {
                //return base.AsyncUnaryCall(request, context, continuation);
                return continuation(request, context);
            }
            catch (Exception e)
            {
                logger.LogError(e, "123");
                throw new RpcException(new Status(StatusCode.Unknown, "A"));
            }
        }
    }
}

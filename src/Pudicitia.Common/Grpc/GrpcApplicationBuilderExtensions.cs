using System.Net.Mime;
using Grpc.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder;

public static class GrpcApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGrpcExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var statusCode = exceptionHandlerPathFeature?.Error switch
                {
                    RpcException rpcException when rpcException.StatusCode == StatusCode.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError,
                };
                var message = exceptionHandlerPathFeature?.Error switch
                {
                    RpcException rpcException when rpcException.StatusCode == StatusCode.NotFound => rpcException.Message,
                    Exception exception => exception.Message,
                    _ => "An exception was throw",
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync(message);
            });
        });

        return app;
    }
}

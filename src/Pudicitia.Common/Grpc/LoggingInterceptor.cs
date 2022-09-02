using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Pudicitia.Common.Domain;

namespace Pudicitia.Common.Grpc;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            if (exception.GetType().IsAssignableToGenericType(typeof(EntityNotFoundException<>)))
            {
                throw new RpcException(new Status(StatusCode.NotFound, exception.Message, exception));
            }

            _logger.LogError(exception, "Context: {@Context} failed.", context);
            throw new RpcException(new Status(StatusCode.Unknown, exception.Message, exception));
        }
    }
}

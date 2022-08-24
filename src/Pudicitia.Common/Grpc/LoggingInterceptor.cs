using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Pudicitia.Common.Grpc;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(request, context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Context: {@Context} failed.", context);
            throw new RpcException(new Status(StatusCode.Unknown, "Request failed.", exception));
        }
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        try
        {
            return continuation(request, context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Context: {@Context} failed.", context);
            throw new RpcException(new Status(StatusCode.Unknown, "Request failed.", exception));
        }
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
            _logger.LogError(exception, "Context: {@Context} failed.", context);
            throw new RpcException(new Status(StatusCode.Unknown, "Unhandled error.", exception));
        }
    }
}

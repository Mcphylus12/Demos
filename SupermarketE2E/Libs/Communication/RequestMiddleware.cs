using System.Net.Http.Json;
using System.Text.Json;
using Communication.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Communication;

public class RequestMiddleware : IMiddleware
{
    private readonly TypeResolutionConfig _typeConfig;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RequestMiddleware> _logger;
    private readonly HandlerManager _handlerManager;

    public RequestMiddleware(
        IOptions<TypeResolutionConfig> typeConfig,
        IServiceProvider serviceProvider,
        ILogger<RequestMiddleware> logger)
    {
        _typeConfig = typeConfig.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _handlerManager = new HandlerManager(_serviceProvider);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.Any(header => header.Key == "X-Request-Type"))
        {
            await next(context);
            return;
        }

        var messageType = context.Request.Headers["X-Request-Type"];

        var requestType = _typeConfig.RequestMap[messageType];

        var requestData = await context.Request.ReadFromJsonAsync(requestType);

        if (requestType.IsAssignableTo(typeof(IRequest<>)))
        {
            var response = await _handlerManager.HandlerWithResponse(requestType, requestData);
            await context.Response.WriteAsJsonAsync(response);
        }
        else
        {
            await _handlerManager.Handler(requestType, requestData);
        }
    }
}

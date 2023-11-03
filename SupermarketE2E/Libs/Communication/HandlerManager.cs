
using Communication.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Communication;

public class HandlerManager
{
    private IServiceProvider _serviceProvider;

    public HandlerManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Handler(Type requestType, object? requestData)
    {
        var jam = (IRequestJam)Activator.CreateInstance(typeof(RequestJam<>).MakeGenericType(requestType));
        await jam.HandleAsync(requestData!, _serviceProvider);
    }

    public async Task MessageHandler(Type requestType, object? requestData)
    {
        var jam = (IMessageJam)Activator.CreateInstance(typeof(MessageJam<>).MakeGenericType(requestType));
        await jam.HandleAsync(requestData!, _serviceProvider);
    }

    public async Task<object> HandlerWithResponse(Type requestType, object requestData)
    {
        var responseType = requestType.GetInterfaces().Single(i => i.Name == typeof(IRequest<>).Name).GetGenericArguments()[0];
        var jam = (IRequestResponseJam)Activator.CreateInstance(typeof(RequestResponseJam<,>).MakeGenericType(requestType, responseType));
        var response = await jam.HandleAsync(requestData!, _serviceProvider);
        return response;
    }
}

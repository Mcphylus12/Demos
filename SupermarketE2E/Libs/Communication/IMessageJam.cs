using Communication.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Communication;

internal interface IMessageJam
{
    public Task HandleAsync(object request, IServiceProvider serviceProvider);
}

internal class MessageJam<TMessage> : IMessageJam
where TMessage : IMessage, new()
{
    public Task HandleAsync(TMessage request, IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>().HandleAsync(request);
    }

    public Task HandleAsync(object request, IServiceProvider serviceProvider)
        => HandleAsync((TMessage)request, serviceProvider);
}

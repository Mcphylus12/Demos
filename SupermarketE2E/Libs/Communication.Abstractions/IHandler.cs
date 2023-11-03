namespace Communication.Abstractions;

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, new()
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<TRequest>
    where TRequest : IRequest, new()
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IMessageHandler<TMessage>
    where TMessage : IMessage, new()
{
    Task HandleAsync(TMessage message, CancellationToken cancellationToken = default);
}

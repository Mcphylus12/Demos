namespace Communication.Abstractions;

public interface ISender
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    Task Send(IRequest request);
    Task Send(IMessage request);
}

public interface IRequest
{
    public string RequestType { get; }
}

public interface IRequest<TResponse> : IRequest
{
}

public interface IMessage
{
    public string MessageType { get; }
}

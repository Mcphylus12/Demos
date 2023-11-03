using Communication.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Communication;

internal interface IRequestResponseJam
{
    public Task<object> HandleAsync(object request, IServiceProvider serviceProvider);
}

internal class RequestResponseJam<TRequest, TResponse> : IRequestResponseJam
where TRequest : IRequest<TResponse>, new()
{

    public Task<TResponse> HandleAsync(TRequest request, IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>().HandleAsync(request);

    public async Task<object> HandleAsync(object request, IServiceProvider serviceProvider)
        => await HandleAsync((TRequest)request, serviceProvider);
}

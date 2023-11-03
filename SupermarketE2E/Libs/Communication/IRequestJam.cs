using Communication.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Communication;

internal interface IRequestJam
{
    public Task HandleAsync(object request, IServiceProvider serviceProvider);
}

internal class RequestJam<TRequest> : IRequestJam
where TRequest : IRequest, new()
{
    public Task HandleAsync(TRequest request, IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IRequestHandler<TRequest>>().HandleAsync(request);

    public Task HandleAsync(object request, IServiceProvider serviceProvider)
        => HandleAsync((TRequest)request, serviceProvider);
}

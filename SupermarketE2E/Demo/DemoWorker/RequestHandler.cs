using Communication.Abstractions;
using DemoCore;

namespace DemoWorker;

public class RequestHandler : IRequestHandler<DemoRequest>
{
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(ILogger<RequestHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DemoRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request Handled Value={value}", request.Data);
        return Task.CompletedTask;
    }
}

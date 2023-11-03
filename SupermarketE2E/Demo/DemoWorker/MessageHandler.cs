using Communication.Abstractions;
using DemoCore;

namespace DemoWorker;

public class MessageHandler : IMessageHandler<DemoMessage>
{
    private readonly ILogger<MessageHandler> _logger;

    public MessageHandler(ILogger<MessageHandler> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(DemoMessage message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Message Handled Value={value}", message.Data);
        return Task.CompletedTask;
    }
}

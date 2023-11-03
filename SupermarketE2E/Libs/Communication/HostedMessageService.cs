using System.Text;
using System.Text.Json;
using Communication.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communication;

internal class HostedMessageService : BackgroundService
{
    private readonly IOptions<CommunicationConfiguration> _options;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IOptions<TypeResolutionConfig> _typeOptions;
    private readonly ILogger<HostedMessageService> _logger;
    private readonly List<Listener> _listeners;
    private readonly HandlerManager _handlerManager;

    public HostedMessageService(
        IOptions<CommunicationConfiguration> options,
        IHostApplicationLifetime applicationLifetime,
        IServiceProvider serviceProvider,
        IOptions<TypeResolutionConfig> typeOptions,
        ILogger<HostedMessageService> logger)
    {
        _options = options;
        _applicationLifetime = applicationLifetime;
        _typeOptions = typeOptions;
        _logger = logger;
        _listeners = new List<Listener>();
        _applicationLifetime.ApplicationStopping.Register(ApplicationStopping);
        _handlerManager = new HandlerManager(serviceProvider);
    }

    private void ApplicationStopping()
    {
        foreach (var listener in _listeners) listener.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var endpointConfig in _options.Value.Inbound.Endpoints)
        {
            _logger.LogInformation("start listener for {endpoint}", endpointConfig.Key);
            _listeners.Add(new Listener(endpointConfig.Key, _typeOptions.Value.MessageMap, _handlerManager));
        }
        Thread.Sleep(Timeout.Infinite);
        return Task.CompletedTask;
    }
}

internal class Listener : IDisposable
{
    private readonly string _endpoint;
    private readonly Dictionary<string, Type> _typeMap;
    private readonly HandlerManager _handlerManager;
    private IConnection _connection;
    private IModel _channel;

    public Listener(string endpoint, Dictionary<string, Type> typeMap, HandlerManager handlerManager)
    {
        var parts = endpoint.Split('~');
        var host = parts[0];
        var queueName = parts[1];
        var factory = new ConnectionFactory() { HostName = host };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: queueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += ProcessMessage;
        _channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);
        _typeMap = typeMap;
        _handlerManager = handlerManager;
    }

    private void ProcessMessage(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            var messageType = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["X-Message-Type"]);
            var message = _typeMap[messageType];
            var request = JsonSerializer.Deserialize(Encoding.UTF8.GetString(e.Body.ToArray()), message);

            _handlerManager.MessageHandler(message, request).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}

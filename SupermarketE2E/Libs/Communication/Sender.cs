using System.Text;
using Communication.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Communication;

internal class Sender : ISender
{
    private readonly Dictionary<string, string>? _requestConfig;
    private readonly Dictionary<string, string>? _messageConfig;
    private readonly ILogger<Sender> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public Sender(
        IOptions<CommunicationConfiguration> options,
        ILogger<Sender> logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        if (options.Value.Outbound?.Requests?.Endpoints is null) _logger.LogInformation("Request Outbound config is null");
        if (options.Value.Outbound?.Messaging?.Endpoints is null) _logger.LogInformation("Messaging Outbound config is null");

        _requestConfig = options.Value.Outbound?.Requests?.Endpoints?
            .SelectMany(kv => kv.Value, (kv, requestType) => new { Key = kv.Key.Replace('#', ':'), requestType })
            .ToDictionary(o => o.requestType, o => o.Key);

        _messageConfig = options.Value.Outbound?.Messaging?.Endpoints?
            .SelectMany(kv => kv.Value, (kv, messageType) => new { Key = kv.Key.Replace('#', ':'), messageType })
            .ToDictionary(o => o.messageType, o => o.Key);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        Guard.NotNull(request);
        Guard.NotNull(_requestConfig);

        if (_requestConfig!.TryGetValue(request.RequestType, out var endpoint))
        {
            return SendRequest(endpoint, request);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for request Type {request.RequestType}");
        }
    }

    private async Task<TResponse> SendRequest<TResponse>(string endpoint, IRequest<TResponse> request)
    {
        var response = await PostRequest(endpoint, request);

        try
        {
            var nResult = JsonConvert.DeserializeObject<TResponse>(await response!.Content.ReadAsStringAsync());
            if (nResult is null) throw new Exception("Deserialisation succeeded but result was null");
            return nResult;
        }
        catch (Exception ex)
        {
            throw new Exception("Error Deserialising request", ex);
        }
    }

    private async Task<HttpResponseMessage> PostRequest(string endpoint, IRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            _logger.LogInformation(request.ToString());
            var content = JsonConvert.SerializeObject(request);
            _logger.LogInformation(content);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("X-Request-Type", request.RequestType);

            var response = await client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception("Error Sending request", ex);
        }
    }

    public Task Send(IRequest request)
    {
        Guard.NotNull(request);
        Guard.NotNull(_requestConfig);

        if (_requestConfig!.TryGetValue(request.RequestType, out var endpoint))
        {
            return PostRequest(endpoint, request);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for request Type {request.RequestType}");
        }
    }

    public Task Send(IMessage message)
    {
        Guard.NotNull(message);
        Guard.NotNull(_messageConfig);

        if (_messageConfig!.TryGetValue(message.MessageType, out var endpoint))
        {
            return SendMessage(endpoint, message);
        }
        else
        {
            throw new InvalidOperationException($"Endpoint not configured for message Type {message.MessageType}");
        }
    }

    private Task SendMessage(string endpoint, IMessage message)
    {
        var parts = endpoint.Split('~');
        var host = parts[0];
        var queueName = parts[1];
        var routingKey = parts[2];
        var factory = new ConnectionFactory() { HostName = host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var headers = channel.CreateBasicProperties();
            headers.Headers = new Dictionary<string, object>();
            headers.Headers.Add("X-Message-Type", message.MessageType);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: "",
                                 routingKey: routingKey,
                                 basicProperties: headers,
                                 body: body);
        }

        return Task.CompletedTask;
    }
}

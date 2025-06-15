using System.Text;
using MessageBus.Events;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;

namespace MessageBus.Publisher;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private const string QueueName = "transactionQueue";
    private readonly IConfiguration _configuration;
    private readonly ILoggerManager _logger;
    private IChannel? _channel;
    private IConnection? _connection;

    public MessageBusClient(IConfiguration configuration, ILoggerManager loggerManager)
    {
        _configuration = configuration;
        _logger = loggerManager;
        _ = SetupRabbitMqAsync();
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }

    public async Task PublishCreateTransactionEventAsync(CreateTransactionEvent createTransactionEvent)
    {
        var message = JsonConvert.SerializeObject(createTransactionEvent);
        if (_connection?.IsOpen == true)
        {
            _logger.LogInfo("RabbitMQ Connection Open, sending message...");
            await SendMessageAsync(message);
        }
        else
        {
            _logger.LogError("RabbitMQ Connection is closed, not sending message.");
        }
    }

    private async Task SendMessageAsync(string message)
    {
        var channel = _channel.EnsureExists();
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(string.Empty, QueueName, false, props, body);
        _logger.LogInfo($"Message published to RabbitMQ: {message}");
    }

    private Task RabbitMQ_ConnectionShutdownAsync(object? sender, ShutdownEventArgs e)
    {
        _logger.LogError("RabbitMQ Connection Shutdown");
        return Task.CompletedTask;
    }

    private async Task SetupRabbitMqAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQHost"].EnsureExists(),
            Port = int.Parse(_configuration["RabbitMQPort"] ?? throw new InvalidOperationException("RabbitMQPort is null"))
        };
        try
        {
            _connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
            _channel = await _connection.CreateChannelAsync().ConfigureAwait(false);
            await _channel.QueueDeclareAsync(QueueName,
                true,
                false,
                false);
            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdownAsync;
            _logger.LogInfo("Connected to MessageBus");
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not connect to Message Bus: {exception.Message}");
            throw;
        }
    }
}

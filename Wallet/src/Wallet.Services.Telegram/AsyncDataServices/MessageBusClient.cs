using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Wallet.Domain.Contracts;
using Wallet.Services.Telegram.Contracts;
using Wallet.Services.Telegram.Dtos;
using Wallet.Shared.Extensions;

namespace Wallet.Services.Telegram.AsyncDataServices;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private const string QueueName = "transactionQueue";
    private readonly IConfiguration _configuration;
    private readonly ILoggerManager _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public MessageBusClient(IConfiguration configuration, ILoggerManager loggerManager)
    {
        _configuration = configuration;
        _logger = loggerManager;
        SetupRabbitMq();
    }

    public async Task PublishNewTransactionAsync(TransactionPublishedDto transactionPublishedDto)
    {
        var message = JsonConvert.SerializeObject(transactionPublishedDto);
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
            DeliveryMode = DeliveryModes.Persistent,
        };

        await channel.BasicPublishAsync(string.Empty, QueueName, false, props, body);
        _logger.LogInfo($"Message published to RabbitMQ: {message}");
    }

    private Task RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogError("RabbitMQ Connection Shutdown");
        return Task.CompletedTask;
    }

    private void SetupRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"].EnsureExists(),
            Port = int.Parse(_configuration["RabbitMQPort"] ?? throw new InvalidOperationException("RabbitMQPort is null")),
        };
        try
        {
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _channel.QueueDeclareAsync(queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
            _logger.LogInfo("Connected to MessageBus");
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not connect to Message Bus: {exception.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
        _channel?.Dispose();
    }
}

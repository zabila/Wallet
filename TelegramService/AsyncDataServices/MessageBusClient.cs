using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TelegramService.Contracts;
using TelegramService.Dtos;

namespace TelegramService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerManager _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public MessageBusClient(IConfiguration configuration, ILoggerManager loggerManager)
    {
        _configuration = configuration;
        _logger = loggerManager;
        SetupRabbitMq();
    }

    public void PublishNewTransaction(TransactionPublishedDto transactionPublishedDto)
    {
        var message = JsonConvert.SerializeObject(transactionPublishedDto);
        if (_connection?.IsOpen == true)
        {
            _logger.LogInfo("RabbitMQ Connection Open, sending message...");
            SendMessage(message);
        }
        else
        {
            _logger.LogError("RabbitMQ Connection is closed, not sending message.");
        }
    }

    public void Dispose()
    {
        _logger.LogInfo("MessageBus Disposed");
        _channel?.Dispose();
        _connection?.Dispose();
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel?.BasicPublish(exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body);

        _logger.LogInfo($"Message published to RabbitMQ: {message}");
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogError("RabbitMQ Connection Shutdown");
    }

    private void SetupRabbitMq()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"] ?? throw new InvalidOperationException("RabbitMQPort is null")),
        };
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _logger.LogInfo("Connected to MessageBus");
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not connect to Message Bus: {exception.Message}");
            throw;
        }
    }
}
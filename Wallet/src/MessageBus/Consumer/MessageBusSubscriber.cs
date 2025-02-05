using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;

namespace MessageBus.Consumer;

public class MessageBusSubscriber : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private readonly ILoggerManager _logger;
    private const string QueueName = "transactionQueue";

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor, ILoggerManager logger)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        _logger = logger;
        InitializeRabbitMqListener();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var channel = _channel.EnsureExists();
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            _logger.LogInfo("Event received from RabbitMQ");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            await _eventProcessor.ProcessEventAsync(notificationMessage, stoppingToken);
        };

        var consumerTag = await channel.BasicConsumeAsync(QueueName, true, consumer, stoppingToken);
        _logger.LogInfo($"Listening for RabbitMQ messages on queue: {QueueName}, consumerTag: {consumerTag}");
    }

    private void InitializeRabbitMqListener()
    {
        var hostName = _configuration["RabbitMQHost"].EnsureExists();
        var port = int.Parse(_configuration["RabbitMQPort"].EnsureExists());
        _logger.LogInfo($"Connecting to RabbitMQ at {hostName}:{port}");

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port,
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
        _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
    }

    private Task RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogError("RabbitMQ Connection Shutdown");
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}

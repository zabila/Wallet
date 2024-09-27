using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Wallet.Domain.Contracts;

namespace Wallet.Integration.MessageBus;

public class MessageBusSubscriber : BackgroundService {
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private readonly ILoggerManager _logger;
    private const string QueueName = "transactionQueue";

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor, ILoggerManager logger) {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        _logger = logger;
        InitializeRabbitMqListener();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, ea) => {
            _logger.LogInfo("Event received from RabbitMQ");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }

    private void InitializeRabbitMqListener() {
        var hostName = _configuration["RabbitMQHost"];
        var port = int.Parse(_configuration["RabbitMQPort"]!);
        _logger.LogInfo($"Connecting to RabbitMQ at {hostName}:{port}");

        var factory = new ConnectionFactory {
            HostName = hostName,
            Port = port
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e) {
        _logger.LogError("RabbitMQ Connection Shutdown");
    }

    public override void Dispose() {
        if (_channel!.IsOpen) {
            _channel.Close();
            _connection!.Close();
        }

        base.Dispose();
    }
}
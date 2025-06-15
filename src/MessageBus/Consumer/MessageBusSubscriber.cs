using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.Abstractions;
using SharedKernel.Extensions;

namespace MessageBus.Consumer;

public class MessageBusSubscriber : BackgroundService
{
    private const string QueueName = "transactionQueue";
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private readonly ILoggerManager _logger;
    private IChannel? _channel;
    private IConnection? _connection;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor, ILoggerManager logger)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;
        _logger = logger;
        // Don't call InitializeRabbitMqListener here, will do it in ExecuteAsync
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        // Initialize RabbitMQ with retries
        const int maxRetries = 5;
        var retryAttempt = 0;
        var connected = false;

        while (!connected && retryAttempt < maxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Initialize RabbitMQ before using the channel
                await InitializeRabbitMqListener();
                connected = true;
                _logger.LogInfo("Successfully connected to RabbitMQ after retries");
            }
            catch (Exception ex)
            {
                retryAttempt++;
                var retryDelay = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)); // Exponential backoff
                _logger.LogWarn($"Failed to connect to RabbitMQ (attempt {retryAttempt}/{maxRetries}): {ex.Message}. Retrying in {retryDelay.TotalSeconds} seconds...");
                
                try
                {
                    await Task.Delay(retryDelay, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Cancellation requested
                    return;
                }
            }
        }
        
        // Now the channel should be initialized
        if (_channel == null)
        {
            _logger.LogError($"Failed to initialize RabbitMQ channel after {maxRetries} attempts");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            _logger.LogInfo("Event received from RabbitMQ");
            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
            await _eventProcessor.ProcessEventAsync(notificationMessage, stoppingToken);
        };

        var consumerTag = await _channel.BasicConsumeAsync(QueueName, true, consumer, stoppingToken);
        _logger.LogInfo($"Listening for RabbitMQ messages on queue: {QueueName}, consumerTag: {consumerTag}");
    }

    private async Task InitializeRabbitMqListener()
    {
        var hostName = _configuration["RabbitMQHost"].EnsureExists();
        var port = int.Parse(_configuration["RabbitMQPort"].EnsureExists());
        _logger.LogInfo($"Connecting to RabbitMQ at {hostName}:{port}");

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port
        };

        try
        {
            _connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
            _channel = await _connection.CreateChannelAsync().ConfigureAwait(false);
            await _channel.QueueDeclareAsync(QueueName, true, false, false);

            _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdownAsync;
            _logger.LogInfo("Successfully connected to RabbitMQ");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to connect to RabbitMQ: {ex.Message}");
            throw;
        }
    }

    private Task RabbitMQ_ConnectionShutdownAsync(object? sender, ShutdownEventArgs e)
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

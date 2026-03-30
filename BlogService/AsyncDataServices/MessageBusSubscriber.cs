using System.Text;
using System.Threading.Channels;
using BlogService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlogService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IChannel _channel;
    private string _queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;

        InitialiseRabbitMQ();
    }

    private async Task InitialiseRabbitMQ()
    {
        if (string.IsNullOrEmpty(_configuration["RabbitMQ:Host"])
            || string.IsNullOrEmpty(_configuration["RabbitMQ:Port"])
        )
        {
            Console.WriteLine("RabbitMQ configuration is missing. Message Bus cannot be initialized.");
            return;
        }

        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:Host"]!,
            Port = int.Parse(_configuration["RabbitMQ:Port"]!)
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout);

        _queueName = _channel.QueueDeclareAsync().Result.QueueName;
        _channel.QueueBindAsync(queue: _queueName,
            exchange: "trigger",
            routingKey: "").Wait();

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdownAsync;

        Console.WriteLine("Listening on Message Bus");
    }

    private Task RabbitMQ_ConnectionShutdownAsync(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ connection shutdown");
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        Console.WriteLine("MessageBus Disposed");
        if (_channel.IsOpen)
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (ModuleHandle, ea) =>
        {
            Console.WriteLine("Event Recieved");

            var body = ea.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}

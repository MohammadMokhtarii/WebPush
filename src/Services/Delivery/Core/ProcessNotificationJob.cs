using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Delivery.Core;

public class ProcessNotificationJob : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IPushNotificationAdapter _pushNotificationAdapter;

    private IConnection _connection;


    public ProcessNotificationJob(IConfiguration configuration, IPushNotificationAdapter pushNotificationAdapter)
    {
        _configuration = configuration;
        _pushNotificationAdapter = pushNotificationAdapter;
        CreateConnectionFactory();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var number in Enumerable.Range(1, 10))
        {
            var _channel = _connection.CreateModel();
            _channel.BasicQos(0, 100, false);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, args) =>
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                await _pushNotificationAdapter.SendAsync(body, stoppingToken);

                _channel.BasicAck(args.DeliveryTag, false);
            };
            _channel.BasicConsume(consumer, "Push");
        }
    }


    private void CreateConnectionFactory()
    {
        string? connectionString = _configuration.GetConnectionString("MessageQueue");
        ArgumentNullException.ThrowIfNull(connectionString);

        ConnectionFactory factory = new()
        {
            Uri = new(connectionString),
            ClientProvidedName = $"WebPush Consumer System - {Environment.MachineName}",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
    }

    public override void Dispose()
    {
        _connection.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}

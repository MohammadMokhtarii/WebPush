using Delivery.Infrastructure.MessageQueues;
using RabbitMQ.Client;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IMessageConsumer, MessageConsumer>();
        services.AddTransient(serviceProvider =>
        {
            //var setting = builder.Configuration.GetSection("App:MessageQueueSettings").Get<MessageQueueSettings>();
            ConnectionFactory factory = new()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = $"Ava Push System - {Environment.MachineName}"
            };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            return channel;
        });
    });

var host = builder.Build();

var messageConsumer = host.Services.GetRequiredService<IMessageConsumer>();

messageConsumer.Consume(async (string body) =>
{
    Console.WriteLine(body);
});
host.Run();



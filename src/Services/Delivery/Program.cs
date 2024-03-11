using Delivery;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();


var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};



var host = builder.Build();
host.Run();



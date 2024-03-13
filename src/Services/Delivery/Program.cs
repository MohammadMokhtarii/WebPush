using Delivery.Model;
using Delivery.Model.Infrastructure.MessageQueues;
using Delivery.Model.Services.Notification;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.RegisterServices(context.Configuration);
    });
var host = builder.Build();

var messageConsumer = host.Services.GetRequiredService<IMessageConsumer>();
var notificationService = host.Services.GetRequiredService<INotificationService>();

messageConsumer.Consume(async (string body) =>
{
    Console.WriteLine(body);
    await notificationService.PushNotification();
});

host.Run();



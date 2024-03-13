using Delivery.Infrastructure.MessageQueues;
using Delivery.Infrastructure.Notification;
using Delivery.Model;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.RegisterServices(context);
    });

var host = builder.Build();

var messageConsumer = host.Services.GetRequiredService<IMessageConsumer>();
var notificationService = host.Services.GetRequiredService<INotificationService>();

messageConsumer.Consume(async (string body) =>
{
    await notificationService.PushNotification();
});

host.Run();



using Delivery.Core;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ProcessNotificationJob>();
        services.AddSingleton<IPushNotificationAdapter, PushNotificationAdapter>();
    });

var host = builder.Build();
host.Run();



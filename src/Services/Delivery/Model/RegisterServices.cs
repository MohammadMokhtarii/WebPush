using Delivery.Model.Dto;
using Delivery.Model.Infrastructure.MessageQueues.Adapter;
using Delivery.Model.Services.Notification;
using RabbitMQ.Client;

namespace Delivery.Model
{

    public static class RegisterServicesExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddTransient<IMessageConsumer, MessageConsumer>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient(serviceProvider =>
            {
                var setting = context.Configuration.GetSection("MessageQueueSettings").Get<MessageQueueSettings>();
                ConnectionFactory factory = new()
                {
                    Uri = new Uri(setting.Url),
                    ClientProvidedName = $"Ava Push System - {Environment.MachineName}"
                };
                IConnection connection = factory.CreateConnection();
                IModel channel = connection.CreateModel();
                return channel;
            });

            return services;
        }
    }
}

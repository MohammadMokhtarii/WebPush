using Delivery.Model.Infrastructure.MessageQueues;
using Delivery.Model.Services.Notification;
using RabbitMQ.Client;

namespace Delivery.Model
{

    public static class RegisterServicesExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMessageConsumer, MessageConsumer>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient(serviceProvider =>
            {
                string? connectionString = configuration.GetConnectionString("MessageQueue");
                ArgumentNullException.ThrowIfNull(connectionString);
                ConnectionFactory factory = new()
                {
                    Uri = new Uri(connectionString),
                    ClientProvidedName = $"Ava Push System Consumer - {Environment.MachineName}"
                };
                IConnection connection = factory.CreateConnection();
                IModel channel = connection.CreateModel();
                return channel;
            });

            return services;
        }
    }
}

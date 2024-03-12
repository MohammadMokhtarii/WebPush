using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Delivery.Infrastructure.MessageQueues
{
    public class MessageConsumer : IMessageConsumer
    {
        readonly IModel _channel;

        protected string ExchangeName => "Ava-Exchange-System";
        protected string RoutingKey => "Ava-Routing-Key";
        protected string QueueName => "Ava-Queue";

        public MessageConsumer(IModel channel)
        {
            _channel = channel;
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(QueueName, false, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
        }
        public void Consume(Func<string, Task> onRecieved)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                onRecieved(body.ToString()).Wait();

                _channel.BasicAck(args.DeliveryTag, false);
            };
            _channel.BasicConsume(QueueName, false, consumer);
        }


    }
}

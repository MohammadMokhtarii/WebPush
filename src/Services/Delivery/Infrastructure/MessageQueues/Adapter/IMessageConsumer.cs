namespace Delivery.Infrastructure.MessageQueues
{
    public interface IMessageConsumer
    {
        void Consume(Func<string, Task> onRecieved);
    }
}

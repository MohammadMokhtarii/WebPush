namespace Delivery.Model.Infrastructure.MessageQueues.Adapter
{
    public interface IMessageConsumer
    {
        void Consume(Func<string, Task> onRecieved);
    }
}

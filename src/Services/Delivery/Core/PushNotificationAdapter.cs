namespace Delivery.Core;

public interface IPushNotificationAdapter
{
    Task SendAsync(object item, CancellationToken cancellationToken = default);

}
public class PushNotificationAdapter : IPushNotificationAdapter
{
    private int number = 0;
    public async Task SendAsync(object item, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1000, cancellationToken);
        number++;
        await Console.Out.WriteLineAsync(DateTime.Now.ToString() + "-----" + number);
    }
}

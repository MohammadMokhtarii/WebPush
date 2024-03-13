namespace Delivery.Model.Services.Notification
{
    public class NotificationService : INotificationService
    {
        public NotificationService()
        {

        }

        public async Task PushNotification()
        {
            await Task.Delay(100);
        }
    }
}

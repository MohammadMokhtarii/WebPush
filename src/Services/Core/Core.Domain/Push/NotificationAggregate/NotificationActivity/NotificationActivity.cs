namespace Core.Domain.Push;

public sealed class NotificationActivity : Entity
{
    private NotificationActivity() { }
    private NotificationActivity(int notificationId, NotificationStatus notificationStatusId, string description)
    {
        NotificationId = notificationId;
        NotificationStatusId = notificationStatusId;
        Description = description;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public int Id { get; private set; }

    public int NotificationId { get; private set; }
    public NotificationStatus NotificationStatusId { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public static NotificationActivity Create(int notificationId, NotificationStatus notificationStatusId, string description) => new(notificationId, notificationStatusId, description);
}

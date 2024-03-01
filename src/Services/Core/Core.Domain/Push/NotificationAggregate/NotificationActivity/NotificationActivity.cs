namespace Core.Domain.Push;

public readonly record struct NotificationActivityId(int Value);
public sealed class NotificationActivity : Entity
{
    private NotificationActivity() { }
    private NotificationActivity(NotificationId notificationId, NotificationStatus notificationStatusId, string description)
    {
        NotificationId = notificationId;
        NotificationStatusId = notificationStatusId;
        Description = description;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public NotificationActivityId Id { get; private set; }

    public NotificationId NotificationId { get; private set; }
    public NotificationStatus NotificationStatusId { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public static NotificationActivity Create(NotificationId notificationId, NotificationStatus notificationStatusId, string description) => new(notificationId, notificationStatusId, description);
}

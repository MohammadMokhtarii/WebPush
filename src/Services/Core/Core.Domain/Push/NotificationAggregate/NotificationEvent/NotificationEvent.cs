namespace Core.Domain.Push;

public sealed class NotificationEvent : Entity
{
    private NotificationEvent() { }
    private NotificationEvent(int notificationId, NotificationEventType eventTypeId)
    {
        NotificationId = notificationId;
        EventTypeId = eventTypeId;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public int Id { get; private set; }

    public int NotificationId { get; private set; }
    public NotificationEventType EventTypeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }


    public static NotificationEvent Create(int notificationId, NotificationEventType eventTypeId)
        => new(notificationId, eventTypeId);

}

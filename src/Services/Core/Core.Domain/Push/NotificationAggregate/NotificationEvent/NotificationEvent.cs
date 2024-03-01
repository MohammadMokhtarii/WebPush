namespace Core.Domain.Push;
public readonly record struct NotificationEventId(int Value);
public sealed class NotificationEvent : Entity
{
    private NotificationEvent() { }
    private NotificationEvent(NotificationId notificationId, NotificationEventType eventTypeId)
    {
        NotificationId = notificationId;
        EventTypeId = eventTypeId;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public NotificationEventId Id { get; private set; }

    public NotificationId NotificationId { get; private set; }
    public NotificationEventType EventTypeId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }


    public static NotificationEvent Create(NotificationId notificationId, NotificationEventType eventTypeId)
        => new(notificationId, eventTypeId);

}

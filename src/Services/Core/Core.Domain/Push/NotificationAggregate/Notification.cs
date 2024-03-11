using Core.Domain.Segment;

namespace Core.Domain.Push;

public readonly record struct NotificationId(int Value);
public sealed class Notification : Entity, IAggregateRoot
{

    private readonly List<NotificationActivity> _notificationActivities = [];
    private readonly List<NotificationEvent> _notificationEvents = [];

    private Notification() { }
    private Notification(DeviceId deviceId, NotificationPayload body)
    {
        DeviceId = deviceId;
        Body = body;
        NotificationStatusId = NotificationStatus.Pending;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public NotificationId Id { get; private set; }

    public DeviceId DeviceId { get; private set; }
    public Device Device { get; private set; } = default!;
    public NotificationPayload Body { get; private set; }
    public NotificationStatus NotificationStatusId { get; set; }
    public DateTime CreatedOnUtc { get; private set; }

    public IReadOnlyCollection<NotificationActivity> NotificationActivities => _notificationActivities;
    public IReadOnlyCollection<NotificationEvent> NotificationEvents => _notificationEvents;

    public static Notification Create(DeviceId deviceId, string title, string message) => new(deviceId, NotificationPayload.Create(title, message));

    public void ChangeStatus(NotificationStatus notificationStatusId, string description)
    {
        if (NotificationStatusId != NotificationStatus.Successful)
        {
            NotificationStatusId = notificationStatusId;

            if (notificationStatusId == NotificationStatus.Successful)
                _notificationEvents.Add(NotificationEvent.Create(Id, NotificationEventType.Sent));

            _notificationActivities.Add(NotificationActivity.Create(Id, notificationStatusId, description));
        }
    }

    public void AddEvent(NotificationEventType notificationEventTypeId)
    {
        if (NotificationStatusId == NotificationStatus.Successful)
            _notificationEvents.Add(NotificationEvent.Create(Id, notificationEventTypeId));
    }

}

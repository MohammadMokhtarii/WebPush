using Core.Domain.Segment;
using Services.Common;

namespace Core.Domain.Push;

public sealed class Notification : Entity, IAggregateRoot
{

    private readonly List<NotificationActivity> _notificationActivities = [];
    private readonly List<NotificationEvent> _notificationEvents = [];

    private Notification() { }
    private Notification(int deviceId, NotificationPayload body)
    {
        DeviceId = deviceId;
        Body = body;
        NotificationStatusId = NotificationStatus.Pending;
        CreatedOnUtc = DateTime.UtcNow;
    }
    public int Id { get; private set; }

    public int DeviceId { get; private set; }
    public Device Device { get; private set; }
    public NotificationPayload Body { get; private set; }
    public NotificationStatus NotificationStatusId { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public IReadOnlyCollection<NotificationActivity> NotificationActivities => _notificationActivities;
    public IReadOnlyCollection<NotificationEvent> NotificationEvents => _notificationEvents;

    public static Result<Notification> Create(int deviceId, string title, string message)
    {
        var payload = NotificationPayload.Create(title, message);
        if (payload.IsFailure)
            return payload.Error;

        return new Notification(deviceId, payload.Data);
    }

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
        => _notificationEvents.Add(NotificationEvent.Create(Id, notificationEventTypeId));

}

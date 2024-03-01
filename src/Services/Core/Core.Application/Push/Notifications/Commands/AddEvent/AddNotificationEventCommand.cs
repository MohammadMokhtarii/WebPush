using Core.Domain.Push;

namespace Core.Application.Push;
public record AddNotificationEventCommand(NotificationId NotificationId, NotificationEventType NotificationEventTypeId) : IRequest<Result>;
using Core.Domain.Push;

namespace Core.Application.Push;
public record AddNotificationEventCommand(int NotificationId, NotificationEventType NotificationEventTypeId) : IRequest<Result>;
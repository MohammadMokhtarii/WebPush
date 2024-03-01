namespace Core.Domain.Push;

public sealed record NotificationAddedDomainEvent(NotificationId NotificationId) : IDomainEvent;

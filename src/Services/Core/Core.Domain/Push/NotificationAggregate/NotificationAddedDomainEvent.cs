namespace Core.Domain.Push;

public sealed record NotificationAddedDomainEvent(int NotificationId) : IDomainEvent;

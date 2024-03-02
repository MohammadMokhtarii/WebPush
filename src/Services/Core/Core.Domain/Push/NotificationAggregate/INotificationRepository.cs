namespace Core.Domain.Push;

public interface INotificationRepository : IRepository<Notification>
{
    Task<Notification?> FindAsync(NotificationId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(NotificationId id, CancellationToken cancellationToken = default);
    Task AddAsync(Notification entity);
}

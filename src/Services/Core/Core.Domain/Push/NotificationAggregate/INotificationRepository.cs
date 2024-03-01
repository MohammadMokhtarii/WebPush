namespace Core.Domain.Push;

public interface INotificationRepository : IRepository<Notification>
{
    Task<Notification?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    void Add(Notification entity);
}

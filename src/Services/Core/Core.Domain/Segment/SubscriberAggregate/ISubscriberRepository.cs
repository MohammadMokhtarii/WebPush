namespace Core.Domain.Segment;

public interface ISubscriberRepository : IRepository<Subscriber>
{
    Task AddAsync(Subscriber entity);

    Task<Subscriber?> FindAsync(SubscriberId id, CancellationToken cancellationToken = default);
    Task<Subscriber?> FindAsync(WebsiteUrl url, CancellationToken cancellationToken = default);
    Task<bool> IsUniqueAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(SubscriberId id, CancellationToken cancellationToken = default);
    Task<bool> DeviceExistsAsync(DeviceId deviceId, SubscriberId subscriberId, CancellationToken cancellationToken = default);



}

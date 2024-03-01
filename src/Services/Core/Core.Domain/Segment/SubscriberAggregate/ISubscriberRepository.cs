namespace Core.Domain.Segment;

public interface ISubscriberRepository : IRepository<Subscriber>
{
    void Add(Subscriber entity);

    Task<Subscriber?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<Subscriber?> FindAsync(WebsiteUrl url, CancellationToken cancellationToken = default);
    Task<bool> IsUniqueAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeviceExistsAsync(int deviceId, int subscriberId, CancellationToken cancellationToken = default);



}

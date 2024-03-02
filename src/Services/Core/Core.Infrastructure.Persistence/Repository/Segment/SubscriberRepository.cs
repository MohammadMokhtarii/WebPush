using Core.Domain.Segment;

namespace Core.Infrastructure.Persistence.Repository;

public class SubscriberRepository(ApplicationDbContext context) : BaseRepository(context), ISubscriberRepository
{
    public async Task AddAsync(Subscriber model)
        => await _context.Subscribers.AddAsync(model);

    public async Task<Subscriber?> FindAsync(SubscriberId id, CancellationToken cancellationToken = default)
        => await _context.Subscribers.FindAsync([id], cancellationToken);

    public async Task<Subscriber?> FindAsync(WebsiteUrl url, CancellationToken cancellationToken = default)
        => await _context.Subscribers.FirstOrDefaultAsync(x => x.Url == url && !x.InActive, cancellationToken);

    public async Task<bool> IsUniqueAsync(string name, CancellationToken cancellationToken = default)
        => !await _context.Subscribers.AnyAsync(x => x.Name == name && !x.InActive, cancellationToken);

    public async Task<bool> ExistsAsync(SubscriberId id, CancellationToken cancellationToken = default)
        => await _context.Subscribers.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> DeviceExistsAsync(DeviceId deviceId, SubscriberId subscriberId, CancellationToken cancellationToken = default)
        => await _context.Subscribers.AnyAsync(x => x.Id == subscriberId && x.Devices.Any(x => x.Id == deviceId), cancellationToken);
}

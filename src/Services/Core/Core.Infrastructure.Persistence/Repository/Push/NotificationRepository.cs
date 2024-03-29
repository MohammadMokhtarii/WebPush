﻿using Core.Domain.Push;

namespace Core.Infrastructure.Persistence.Repository;

public class NotificationRepository(ApplicationDbContext context) : BaseRepository(context), INotificationRepository
{
    public async Task AddAsync(Notification model)
        => await _context.Notifications.AddAsync(model);

    public async Task<Notification?> FindAsync(NotificationId id, CancellationToken cancellationToken = default)
        => await _context.Notifications.Include(x => x.Device).FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);


    public async Task<bool> ExistsAsync(NotificationId id, CancellationToken cancellationToken = default)
        => await _context.Notifications.AnyAsync(x => x.Id == id, cancellationToken: cancellationToken);

}

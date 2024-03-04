using Core.Application.Common;
using Core.Application.Segment;
using Core.Domain.Push;

namespace Core.Application.Push;
public class AddNotificationEventCommandHandler(IUnitOfWork uow, INotificationRepository notificationRepository) : IRequestHandler<AddNotificationEventCommand, Result>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    public async Task<Result> Handle(AddNotificationEventCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.FindAsync(request.NotificationId, cancellationToken);
        if (notification is null)
            return PushApplicationErrors.InvalidNotification;

        notification.AddEvent(request.NotificationEventTypeId);

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return dbResult.Error;

        return Result.Success();
    }
}

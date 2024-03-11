using Core.Application.Common;
using Core.Domain.Push;

namespace Core.Application.Push;
public class SendNotificationCommandHandler(IUnitOfWork uow, INotificationRepository notificationRepository, IPublisher publisher) : IRequestHandler<SendNotificationCommand, Result<int>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    public async Task<Result<int>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var model = Notification.Create(request.DeviceId, request.Payload.Title, request.Payload.Message);

        await _notificationRepository.AddAsync(model);

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return dbResult.Error;

        //TODO:Will Be Removed At Final Product
        await publisher.Publish(new NotificationAddedDomainEvent(model.Id), cancellationToken);

        return model.Id.Value;
    }
}

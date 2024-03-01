using Core.Application.Common;
using Core.Domain.Push;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Core.Application.Push;

public class NotificationAddedDomainEventHandler(ILogger<NotificationAddedDomainEventHandler> logger,
                                                 IUnitOfWork uow,
                                                 INotificationRepository notificationRepository,
                                                 IWebPushAdapter pushNotificationAdapter) : INotificationHandler<NotificationAddedDomainEvent>
{
    private readonly ILogger<NotificationAddedDomainEventHandler> _logger = logger;
    private readonly IUnitOfWork _uow = uow;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IWebPushAdapter _pushNotificationAdapter = pushNotificationAdapter;
    public async Task Handle(NotificationAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        var model = await _notificationRepository.FindAsync(notification.NotificationId, cancellationToken);
        if (model is null)
            _logger.LogWarning("");

        var strMessage = JsonSerializer.Serialize(new
        {
            model!.Id,
            model!.Body.Title,
            model!.Body.Message,
        });

        var result = await _pushNotificationAdapter.SendAsync(model.Device.PushManager, strMessage, cancellationToken);
        if (result.IsSucess)
            model.ChangeStatus(NotificationStatus.Successful, "");
        else
            model.ChangeStatus(NotificationStatus.Failed, result.Error.ToString());

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            throw new Exception(dbResult.Error.Message);
    }
}

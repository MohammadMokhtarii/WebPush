using Core.Application.Push;
using Core.Domain.Push;
using Core.Domain.Segment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers.V1;


public class NotificationsController(ISender sender) : BaseController
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> SendAsync(SendNotificationCommand.SendNotificationDto item, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new SendNotificationCommand(new(item.DeviceId),new(item.SubscriberId),item.Payload), cancellationToken));


    [HttpPost("{id}/event/{eventType}")]
    public async Task<IActionResult> AddEventAsync(int id, NotificationEventType eventType, CancellationToken cancellationToken)
      => Ok(await _sender.Send(new AddNotificationEventCommand(new NotificationId(id), eventType), cancellationToken));
}


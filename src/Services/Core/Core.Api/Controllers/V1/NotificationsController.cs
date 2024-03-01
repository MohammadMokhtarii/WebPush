using Core.Application.Push;
using Core.Domain.Push;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers.V1;


public class NotificationsController(ISender sender) : BaseController
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> SendAsync(SendNotificationCommand item, CancellationToken cancellationToken)
        => Ok(await _sender.Send(item, cancellationToken));


    [HttpPost("{id}/event/{eventType}")]
    public async Task<IActionResult> AddEventAsync(int id, NotificationEventType eventType, CancellationToken cancellationToken)
      => Ok(await _sender.Send(new AddNotificationEventCommand(id, eventType), cancellationToken));
}


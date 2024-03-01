using Core.Api.Core;
using Core.Application.Segment;
using Core.Domain.Segment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Core.Application.Segment.AddDeviceCommand;

namespace Core.Api.Controllers.V1;

public class DevicesController(ISender sender) : BaseController
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddDeviceDto item, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new AddDeviceCommand(item.Name, item.PushManager, HttpContext.ExtractClientMetadata(), new SubscriberId(item.SubscriberId)), cancellationToken));

}


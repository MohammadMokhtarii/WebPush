using Core.Application.Segment;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Core.Api.Controllers.V1;


public class SubscribersController(ISender sender) : BaseController
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<IActionResult> ConfigAsync(CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetSubscriberConfigQuery(HttpContext.Request.Headers.Origin), cancellationToken));
}

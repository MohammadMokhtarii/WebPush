using Core.Application.Segment;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Core.Api.Controllers.V1;


public class SubscribersController(ISender sender) : BaseController
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<IActionResult> ConfigAsync(CancellationToken cancellationToken)
    {
        var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        return Ok(await _sender.Send(new GetSubscriberConfigQuery(url), cancellationToken));
    }
}

using static Core.Application.Segment.GetSubscriberConfigQuery;

namespace Core.Application.Segment;

public sealed record GetSubscriberConfigQuery(string Url) : IRequest<Result<SubscriberConfigDto>>
{
    public sealed record SubscriberConfigDto(int SubscriberId, Guid Token);
}



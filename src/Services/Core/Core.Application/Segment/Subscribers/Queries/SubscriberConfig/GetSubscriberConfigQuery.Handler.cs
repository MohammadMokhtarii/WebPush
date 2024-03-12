using Core.Domain.Segment;
using static Core.Application.Segment.GetSubscriberConfigQuery;

namespace Core.Application.Segment;

public sealed class GetSubscriberConfigQueryHandler(ISubscriberRepository subscriberRepository) : IRequestHandler<GetSubscriberConfigQuery, Result<SubscriberConfigDto>>
{
    private readonly ISubscriberRepository _subscriberRepository = subscriberRepository;
    public async Task<Result<SubscriberConfigDto>> Handle(GetSubscriberConfigQuery request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.FindAsync(WebsiteUrl.Create(request.Url), cancellationToken);
        if (subscriber is null || subscriber.InActive)
            return SegmentApplicationErrors.InvalidSubscriber;

        return new SubscriberConfigDto(subscriber.Id.Value, subscriber.Token);
    }
}

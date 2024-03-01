using Core.Domain.Segment;
using static Core.Application.Segment.GetSubscriberConfigQuery;

namespace Core.Application.Segment;

public sealed class GetSubscriberConfigQueryHandler(ISubscriberRepository subscriberRepository) : IRequestHandler<GetSubscriberConfigQuery, Result<SubscriberConfigDto>>
{
    private readonly ISubscriberRepository _subscriberRepository = subscriberRepository;
    public async Task<Result<SubscriberConfigDto>> Handle(GetSubscriberConfigQuery request, CancellationToken cancellationToken)
    {
        var url = WebsiteUrl.Create(request.Url);
        if (url.IsFailure)
            return url.Error;

        var subscriber = await _subscriberRepository.FindAsync(url.Data, cancellationToken);
        if (subscriber is null || subscriber.InActive)
            return Error.NotFound(nameof(AppResource.NotFound), string.Format(AppResource.NotFound, request.Url));

        return new SubscriberConfigDto(subscriber.Id.Value, subscriber.Token);
    }
}

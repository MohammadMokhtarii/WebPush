using Core.Application.Common;
using Core.Domain.Segment;

namespace Core.Application.Segment;

public class DeActivateSubscriberCommandHandler(IUnitOfWork uow, ISubscriberRepository subscriberRepository) : IRequestHandler<DeActivateSubscriberCommand, Result>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ISubscriberRepository _subscriberRepository = subscriberRepository;
    public async Task<Result> Handle(DeActivateSubscriberCommand request, CancellationToken cancellationToken)
    {
        var model = await _subscriberRepository.FindAsync(request.Id, cancellationToken);
        if (model is null)
            return SegmentApplicationErrors.InvalidSubscriber;

        model.DeActivate();

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return dbResult.Error;

        return Result.Success();
    }
}

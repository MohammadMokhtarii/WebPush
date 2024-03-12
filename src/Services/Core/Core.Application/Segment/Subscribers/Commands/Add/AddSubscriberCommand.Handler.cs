using Core.Application.Common;
using Core.Domain.Segment;

namespace Core.Application.Segment;

public class AddSubscriberCommandHandler(IUnitOfWork uow, ISubscriberRepository subscriberRepository) : IRequestHandler<AddSubscriberCommand, Result<int>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ISubscriberRepository _subscriberRepository = subscriberRepository;
    public async Task<Result<int>> Handle(AddSubscriberCommand request, CancellationToken cancellationToken)
    {
        var model = Subscriber.Create(request.Name, request.WebsiteUrl);

        await _subscriberRepository.AddAsync(model);

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return dbResult.Error;

        return model.Id.Value;
    }
}

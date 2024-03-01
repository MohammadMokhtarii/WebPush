using Core.Application.Common;
using Core.Domain.Segment;

namespace Core.Application.Segment;

public class AddDeviceCommandHandler(IUnitOfWork uow, ISubscriberRepository subscriberRepository) : IRequestHandler<AddDeviceCommand, Result<int>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly ISubscriberRepository _subscriberRepository = subscriberRepository;
    public async Task<Result<int>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var subscriber = await _subscriberRepository.FindAsync(request.SubscriberId, cancellationToken);
        if (subscriber is null)
            return SegmentApplicationErrors.Device.InvalidSubscriber;

        var device = subscriber.AddDevice(request.Name, request.PushManager, request.ClientMetadata);
        if (device.IsFailure)
            return device.Error;

        var dbResult = await _uow.SaveChangesAsync(cancellationToken);
        if (dbResult.IsFailure)
            return dbResult.Error;

        return device.Data.Id.Value;
    }
}

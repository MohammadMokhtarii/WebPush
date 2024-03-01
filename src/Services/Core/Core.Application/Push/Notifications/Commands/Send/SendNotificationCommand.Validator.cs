using Core.Domain.Segment;

namespace Core.Application.Push;

public class SendNotificationCommandValidator : AbstractValidator<SendNotificationCommand>
{
    private readonly ISubscriberRepository _subscriberRepository;
    public SendNotificationCommandValidator(ISubscriberRepository deviceRepository)
    {
        _subscriberRepository = deviceRepository;

        RuleFor(x => x.DeviceId).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                .MustAsync(async (command, deviceId, cancellationToken) => await BeValidDevice(deviceId,command.SubscriberId, cancellationToken)).WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

        RuleFor(x => x.Payload).ChildRules(child =>
        {
            child.RuleFor(x => x.Title).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                       .MaximumLength(150).WithMessage(string.Format(AppResource.MaxLengthExceeded, 150)).WithErrorCode(nameof(AppResource.MaxLengthExceeded));

            child.RuleFor(x => x.Message).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                         .MaximumLength(500).WithMessage(string.Format(AppResource.MaxLengthExceeded, 500)).WithErrorCode(nameof(AppResource.MaxLengthExceeded));
        });


    }
    private async Task<bool> BeValidDevice(int deviceId, int subscriberId, CancellationToken cancellationToken = default) => await _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, cancellationToken);

}






using Core.Domain.Segment;

namespace Core.Application.Segment;

public class AddDeviceCommandValidator : AbstractValidator<AddDeviceCommand>
{
    private readonly ISubscriberRepository _subscriberRepository;
    public AddDeviceCommandValidator(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;

        RuleFor(x => x.Name).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                            .MaximumLength(50).WithMessage(string.Format(AppResource.MaxLengthExceeded, 50)).WithErrorCode(nameof(AppResource.MaxLengthExceeded));

        RuleFor(x => x.SubscriberId).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                    .MustAsync(BeValidSubscriber).When(x => x.SubscriberId != default!).WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

        RuleFor(x => x.PushManager).ChildRules(child =>
        {
            child.RuleFor(x => x.Endpoint).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty));
            child.RuleFor(x => x.Auth).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty));
            child.RuleFor(x => x.P256DH).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty));
        });

        RuleFor(x => x.ClientMetadata).ChildRules(child =>
        {
            child.RuleFor(x => x.OS).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty));
        });


    }
    private async Task<bool> BeValidSubscriber(SubscriberId id, CancellationToken cancellationToken = default) => await _subscriberRepository.ExistsAsync(id, cancellationToken);

}






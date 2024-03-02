using Core.Domain.Segment;

namespace Core.Application.Segment;

public class DeActivateSubscriberCommandValidator : AbstractValidator<DeActivateSubscriberCommand>
{

    private readonly ISubscriberRepository _subscriberRepository;
    public DeActivateSubscriberCommandValidator(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
                          .NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                          .MustAsync(BeValidSubscriber).WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound))
                          .MustAsync(BeActiveSubscriber).WithMessage(AppResource.MustBeActive).WithErrorCode(nameof(AppResource.MustBeActive));

    }


    private async Task<bool> BeValidSubscriber(SubscriberId id, CancellationToken cancellationToken = default) => await _subscriberRepository.ExistsAsync(id, cancellationToken);
    private async Task<bool> BeActiveSubscriber(SubscriberId id, CancellationToken cancellationToken = default)
    {
        var subscriber = await _subscriberRepository.FindAsync(id, cancellationToken);
        return subscriber is not null && !subscriber.InActive;
    }
}
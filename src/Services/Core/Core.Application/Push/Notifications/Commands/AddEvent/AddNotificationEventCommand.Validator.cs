using Core.Domain.Push;

namespace Core.Application.Push;

public class AddNotificationEventCommandValidator : AbstractValidator<AddNotificationEventCommand>
{
    private readonly INotificationRepository _notificationRepository;
    public AddNotificationEventCommandValidator(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;

        RuleFor(x => x.NotificationId).Cascade(CascadeMode.Stop)
                                      .NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                      .MustAsync(BeValidNotification).WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

        RuleFor(x => x.NotificationEventTypeId).Cascade(CascadeMode.Stop)
                                               .NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                               .IsInEnum().WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

    }
    private async Task<bool> BeValidNotification(NotificationId id, CancellationToken cancellationToken = default) => await _notificationRepository.ExistsAsync(id, cancellationToken);

}






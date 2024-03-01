using Core.Domain.Push;

namespace Core.Application.Push;

public class AddNotificationEventCommandValidator : AbstractValidator<AddNotificationEventCommand>
{
    private readonly INotificationRepository _notificationRepository;
    public AddNotificationEventCommandValidator(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;

        RuleFor(x => x.NotificationId).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                      .MustAsync(BeValidDevice).WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

        RuleFor(x => x.NotificationEventTypeId).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                               .IsInEnum().WithMessage((_, value) => string.Format(AppResource.NotFound, value)).WithErrorCode(nameof(AppResource.NotFound));

    }
    private async Task<bool> BeValidDevice(int id, CancellationToken cancellationToken = default) => await _notificationRepository.ExistsAsync(id, cancellationToken);

}






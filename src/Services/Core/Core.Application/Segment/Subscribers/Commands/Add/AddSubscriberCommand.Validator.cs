using Core.Domain.Segment;

namespace Core.Application.Segment;

public class AddSubscriberCommandValidator : AbstractValidator<AddSubscriberCommand>
{

    private readonly ISubscriberRepository _subscriberRepository;
    public AddSubscriberCommandValidator(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;

        RuleFor(x => x.Name).NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                            .MaximumLength(50).WithMessage(string.Format(AppResource.MaxLengthExceeded, 50)).WithErrorCode(nameof(AppResource.MaxLengthExceeded))
                            .MustAsync(BeUniqueName).WithMessage((_, value) => string.Format(AppResource.MustBeUnique, value)).WithErrorCode(nameof(AppResource.MustBeUnique));

        RuleFor(x => x.WebsiteUrl).Cascade(CascadeMode.Stop)
                                  .NotEmpty().WithMessage(AppResource.NotEmpty).WithErrorCode(nameof(AppResource.NotEmpty))
                                  .MaximumLength(50).WithMessage(string.Format(AppResource.MaxLengthExceeded, 50)).WithErrorCode(nameof(AppResource.MaxLengthExceeded))
                                  .Must(BeValidUrl).WithMessage(AppResource.MustBeValid).WithErrorCode(nameof(AppResource.MustBeValid));


    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken = default) => await _subscriberRepository.IsUniqueAsync(name, cancellationToken);

    private bool BeValidUrl(string url) => UrlFormatChecker.UrlRegex().IsMatch(url);
}

using Core.Domain.Exceptions;
using Services.Common;

namespace Core.Domain.Push;

public readonly record struct NotificationPayload
{
    public const int Title_MaxLength = 150;
    public const int Message_MaxLength = 500;
    public NotificationPayload() { }
    private NotificationPayload(string title, string message)
    {
        Title = title;
        Message = message;
    }
    public string Title { get; init; } = default!;
    public string Message { get; init; } = default!;

    public static NotificationPayload Create(string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new NotificationPayloadIsInvalidDomainException("TitleCanNotBeEmpty");

        if (string.IsNullOrWhiteSpace(message))
            throw new NotificationPayloadIsInvalidDomainException("MessageCanNotBeEmpty");

        if (title.Length > Title_MaxLength)
            throw new NotificationPayloadIsInvalidDomainException("TitleMaxLengthExcced");


        if (message.Length > Message_MaxLength)
            throw new NotificationPayloadIsInvalidDomainException("MessageMaxLengthExcced");


        return new NotificationPayload(title, message);
    }
}

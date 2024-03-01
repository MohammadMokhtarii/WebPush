using Services.Common;

namespace Core.Domain.Push;

public readonly record struct NotificationPayload
{
    public const int Title_MaxLength = 150;
    public const int Message_MaxLength = 500;
    private NotificationPayload(string title, string message)
    {
        Title = title;
        Message = message;
    }
    public string Title { get; init; }
    public string Message { get; init; }

    public static Result<NotificationPayload> Create(string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title))
            return PushDomainErrors.Notification.NotificationPayload.TitleCanNotBeEmpty;

        if (string.IsNullOrWhiteSpace(message))
            return PushDomainErrors.Notification.NotificationPayload.MessageCanNotBeEmpty;

        if (title.Length > Title_MaxLength)
            return PushDomainErrors.Notification.NotificationPayload.TitleMaxLengthExcced;

        if (message.Length > Message_MaxLength)
            return PushDomainErrors.Notification.NotificationPayload.MessageMaxLengthExcced;

        return new NotificationPayload(title, message);
    }
}

using Services.Common;

namespace Core.Domain.Push;
internal static class PushDomainErrors
{
    internal static class Notification
    {
        internal static class NotificationPayload
        {
            public readonly static Error TitleCanNotBeEmpty = Error.Validation("", "");
            public readonly static Error MessageCanNotBeEmpty = Error.Validation("", "");

            public readonly static Error TitleMaxLengthExcced = Error.Validation("", "");
            public readonly static Error MessageMaxLengthExcced = Error.Validation("", "");
        }
    }
}

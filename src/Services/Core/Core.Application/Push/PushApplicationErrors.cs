namespace Core.Application.Segment;

internal static class PushApplicationErrors
{
    public readonly static Error InvalidNotification = Error.Validation("dsa", "ds");

    internal static class Notification
    {
    }
}

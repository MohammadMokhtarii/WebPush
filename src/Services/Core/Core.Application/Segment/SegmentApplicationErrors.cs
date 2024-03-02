namespace Core.Application.Segment;

internal static class SegmentApplicationErrors
{
    public readonly static Error InvalidSubscriber = Error.Validation("dsa", "ds");

    internal static class Device
    {
    }

    internal static class Subscriber
    {
    }
}

namespace Core.Application.Segment;

internal static class SegmentApplicationErrors
{
    internal static class Device
    {
        public readonly static Error InvalidSubscriber = Error.Validation("dsa", "ds");
    }
}

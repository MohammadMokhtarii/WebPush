using Services.Common;

namespace Core.Domain.Segment;

internal static class SegmentDomainErrors
{
    internal static class Subscriber
    {
        public readonly static Error SubscriberIsInActive = Error.Validation("dsa", "ds");
        public readonly static Error SubscriberIsAlreadyInActive = Error.Validation("dsa", "ds");
        internal static class WebsiteUrl
        {
            public readonly static Error InvalidUrl = Error.Validation("dsa", "ds");
        }
    }
}
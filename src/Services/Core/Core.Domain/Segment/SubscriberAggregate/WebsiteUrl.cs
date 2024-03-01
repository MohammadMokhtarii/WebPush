using Services.Common;

namespace Core.Domain.Segment;

public readonly record struct WebsiteUrl
{
    private WebsiteUrl(string url)
    {
        Url = url;
    }
    public string Url { get; init; }


    public static Result<WebsiteUrl> Create(string url)
    {
        if (!UrlFormatChecker.UrlRegex().IsMatch(url))
            return SegmentDomainErrors.Subscriber.WebsiteUrl.InvalidUrl;

        return new WebsiteUrl(url);
    }

}
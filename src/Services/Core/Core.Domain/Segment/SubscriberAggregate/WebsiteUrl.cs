using Core.Domain.Exceptions;
using Services.Common;

namespace Core.Domain.Segment;

public readonly record struct WebsiteUrl
{
    public WebsiteUrl() { }
    private WebsiteUrl(string url)
    {
        Value = url;
    }
    public string Value { get; init; } = default!;


    public static WebsiteUrl Create(string url)
    {
        if (!UrlFormatChecker.UrlRegex().IsMatch(url))
            throw new SubscriberUrlIsInvalidDomainException($"{url} Is Invalid");

        return new WebsiteUrl(url);
    }

}
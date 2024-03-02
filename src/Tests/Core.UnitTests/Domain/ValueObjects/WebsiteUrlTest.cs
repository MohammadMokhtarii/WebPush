using Core.Domain.Segment;
using Services.Common;

namespace Core.UnitTests.Domain.ValueObjects;


public class WebsiteUrlTest
{

    [Fact]
    public void Create_Should_ReturnInvalidUrl_When_UrlIsInvalid()
    {
        //Arrange
        string url = "example.com";

        //Act
        var result = WebsiteUrl.Create(url);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.WebsiteUrl.InvalidUrl);
    }

    [Fact]
    public void Create_Should_ReturnWebsiturl_When_UrlIsValid()
    {
        //Arrange
        string url = "https://example.com";

        //Act
        var result = WebsiteUrl.Create(url);

        //Assert
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Data.Url.Should().Be(url);
    }
}


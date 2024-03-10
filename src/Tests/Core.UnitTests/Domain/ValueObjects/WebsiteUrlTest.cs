using Core.Domain.Exceptions;
using Core.Domain.Segment;
using NSubstitute.ExceptionExtensions;
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
        var result = () => WebsiteUrl.Create(url);

        //Assert
        result.Should().Throw<SubscriberUrlIsInvalidDomainException>();
    }

    [Fact]
    public void Create_Should_ReturnWebsiturl_When_UrlIsValid()
    {
        //Arrange
        string url = "https://example.com";

        //Act
        var result = () => WebsiteUrl.Create(url);

        //Assert
        result.Should().NotThrow<SubscriberUrlIsInvalidDomainException>();
        result().Value.Should().Be(url);

    }
}


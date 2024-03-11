using Core.Domain.Exceptions;
using Core.Domain.Segment;
using NSubstitute.ExceptionExtensions;
using Services.Common;

namespace Core.UnitTests.Domain.Aggregates;

public class SubscriberTest
{

    [Fact]
    public void Create_Should_ReturnInValidUrl_When_UrlIsInValid()
    {
        //Arrange
        string name = "Test";
        string url = "example.com";

        //Act
        var result = () => Subscriber.Create(name, url);

        //Assert
        result.Should().Throw<SubscriberUrlIsInvalidDomainException>();
    }
    [Fact]
    public void Create_Should_ReturnSubscriber_When_UrlIsValid()
    {
        //Arrange
        string name = "Test";
        string url = "https://example.com";

        //Act
        var result = () => Subscriber.Create(name, url);

        //Assert
        result.Should().NotThrow<SubscriberUrlIsInvalidDomainException>();
        result().Url.Value.Should().Be(url);
    }

    [Fact]
    public void DeActivate_Should_ReturnSucess_When_SubscriberIsActive()
    {
        //Arrange
        string name = "Test";
        string url = "https://example.com";
        var subscriber = Subscriber.Create(name, url);

        //Act
        subscriber.DeActivate();

        //Assert
        subscriber.InActive.Should().BeTrue();
    }

    [Fact]
    public void AddDevice_Should_ReturnSubscriberIsInActive_When_SubscriberIsInActive()
    {
        //Arrange
        string subscriberName = "FakeName";
        string subscriberUrl = "https://example.com";
        var subscriber = Subscriber.Create(subscriberName, subscriberUrl);
        subscriber.DeActivate();

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");

        //Act
        var result = () => subscriber.AddDevice(deviceName, devicePushManager, deviceClientMetadata);

        //Assert
        result.Should().Throw<SubscriberIsInActiveDomainException>();
        subscriber.Devices.Should().BeEmpty();
    }

    [Fact]
    public void AddDevice_Should_ReturnDeviceId_When_SubscriberIsActive()
    {
        //Arrange
        string subscriberName = "FakeName";
        string subscriberUrl = "https://example.com";
        var subscriber = Subscriber.Create(subscriberName, subscriberUrl);

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");

        //Act
        var result = () => subscriber.AddDevice(deviceName, devicePushManager, deviceClientMetadata);

        //Assert
        result.Should().NotThrow<SubscriberIsInActiveDomainException>();
        subscriber.Devices.Any(x => x.Id == result().Id).Should().BeTrue();

    }
}

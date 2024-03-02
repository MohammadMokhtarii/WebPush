using Core.Domain.Segment;
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
        var result = Subscriber.Create(name, url);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.WebsiteUrl.InvalidUrl);
    }
    [Fact]
    public void Create_Should_ReturnSubscriber_When_UrlIsValid()
    {
        //Arrange
        string name = "Test";
        string url = "https://example.com";

        //Act
        var result = Subscriber.Create(name, url);

        //Assert
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Data.Name.Should().Be(name);
        result.Data.Url.Url.Should().Be(url);
    }


    [Fact]
    public void DeActivate_Should_ReturnSubscriberIsAlreadyInActive_When_SubscriberIsInActive()
    {
        //Arrange
        string name = "Test";
        string url = "https://example.com";
        var subscriber = Subscriber.Create(name, url);
        subscriber.Data.DeActivate();

        //Act
        var result = subscriber.Data.DeActivate();

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.SubscriberIsAlreadyInActive);
    }

    [Fact]
    public void DeActivate_Should_ReturnSucess_When_SubscriberIsActive()
    {
        //Arrange
        string name = "Test";
        string url = "https://example.com";
        var subscriber = Subscriber.Create(name, url);

        //Act
        var result = subscriber.Data.DeActivate();

        //Assert
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        subscriber.Data.InActive.Should().BeTrue();
    }

    [Fact]
    public void AddDevice_Should_ReturnSubscriberIsInActive_When_SubscriberIsInActive()
    {
        //Arrange
        string subscriberName = "FakeName";
        string subscriberUrl = "https://example.com";
        var subscriber = Subscriber.Create(subscriberName, subscriberUrl);
        subscriber.Data.DeActivate();

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");

        //Act
        var result = subscriber.Data.AddDevice(deviceName, devicePushManager, deviceClientMetadata);

        //Assert
        result.IsFailure.Should().BeTrue();

        subscriber.Data.Devices.Should().BeEmpty();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.SubscriberIsInActive);
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
        var result = subscriber.Data.AddDevice(deviceName, devicePushManager, deviceClientMetadata);

        //Assert
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        subscriber.Data.Devices.Any(x => x.Id == result.Data.Id).Should().BeTrue();

    }
}

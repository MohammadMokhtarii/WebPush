using Core.Application.Segment;
using Core.Domain.Segment;
using Services.Common;

namespace Core.UnitTests.Application.Segment.Subscribers;

public class GetSubscriberConfigQueryTest
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly GetSubscriberConfigQueryHandler _handler;

    public GetSubscriberConfigQueryTest()
    {
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _handler = new(_subscriberRepository);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_UrlIsInvalid()
    {
        //Arrange   
        string url = "http:dd//Example.com";
        GetSubscriberConfigQuery command = new(url);
        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.WebsiteUrl.InvalidUrl);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInvalid()
    {
        //Arrange   
        string url = "https://Example.com";
        GetSubscriberConfigQuery command = new(url);

        _subscriberRepository.FindAsync(Arg.Any<WebsiteUrl>()).ReturnsNull();

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<WebsiteUrl>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentApplicationErrors.InvalidSubscriber);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInActive()
    {
        //Arrange   
        string url = "https://Example.com";
        GetSubscriberConfigQuery command = new(url);
        Subscriber subscriber = Subscriber.Create("FakeName", url).Data;
        subscriber.DeActivate();

        _subscriberRepository.FindAsync(Arg.Any<WebsiteUrl>()).Returns(subscriber);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<WebsiteUrl>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentApplicationErrors.InvalidSubscriber);
    }

    [Fact]
    public async Task Handle_Should_ReturnConfig_When_SubscriberIsValid()
    {
        //Arrange   
        string url = "https://Example.com";
        GetSubscriberConfigQuery command = new(url);
        Subscriber subscriber = Subscriber.Create("FakeName", url).Data;

        _subscriberRepository.FindAsync(Arg.Any<WebsiteUrl>()).Returns(subscriber);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<WebsiteUrl>());

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);

        result.Data.SubscriberId.Should().Be(subscriber.Id.Value);
        result.Data.Token.Should().Be(subscriber.Token);
    }
}

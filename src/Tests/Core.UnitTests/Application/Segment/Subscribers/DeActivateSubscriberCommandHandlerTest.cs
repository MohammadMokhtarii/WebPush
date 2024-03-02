using Core.Application.Common;
using Core.Application.Segment;
using Core.Domain.Segment;
using Services.Common;

namespace Core.UnitTests.Application.Segment.Subscribers;

public class DeActivateSubscriberCommandHandlerTest
{
    private readonly IUnitOfWork _uow;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly DeActivateSubscriberCommandHandler _handler;

    public DeActivateSubscriberCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _handler = new(_uow, _subscriberRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInvalid()
    {
        //Arrange   
        int subscriberId = 10;
        DeActivateSubscriberCommand command = new(new SubscriberId(subscriberId));

        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>()).ReturnsNull();

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<SubscriberId>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentApplicationErrors.InvalidSubscriber);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInActive()
    {
        //Arrange   
        int subscriberId = 10;
        string subscriberName = "FakeName";
        string url = "https://example.com";
        DeActivateSubscriberCommand command = new(new SubscriberId(subscriberId));
        Subscriber subscriber = Subscriber.Create(subscriberName, url).Data;
        subscriber.DeActivate();
        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>()).Returns(subscriber);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<SubscriberId>());
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.SubscriberIsAlreadyInActive);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SaveChangesFailed()
    {
        //Arrange   
        int subscriberId = 10;
        string subscriberName = "FakeName";
        string url = "https://example.com";
        DeActivateSubscriberCommand command = new(new SubscriberId(subscriberId));
        Subscriber subscriber = Subscriber.Create(subscriberName, url).Data;

        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>()).Returns(subscriber);
        _uow.SaveChangesAsync(default).Returns(UnitOfWorkErrors.SaveChangesError);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _uow.Received().SaveChangesAsync(default);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    }

    [Fact]
    public async Task Handle_Should_ReturnSubscriberId_When_SaveChangesSucessed()
    {
        //Arrange   
        int subscriberId = 10;
        string subscriberName = "FakeName";
        string url = "https://example.com";
        DeActivateSubscriberCommand command = new(new SubscriberId(subscriberId));
        Subscriber subscriber = Subscriber.Create(subscriberName, url).Data;

        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>()).Returns(subscriber);
        _uow.SaveChangesAsync(default).Returns(Result.Success());

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _uow.Received().SaveChangesAsync(default);
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }
}

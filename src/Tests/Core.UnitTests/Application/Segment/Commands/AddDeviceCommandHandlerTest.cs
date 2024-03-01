using Core.Application.Common;
using Core.Application.Segment;
using Core.Domain.Segment;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Services.Common;

namespace Core.UnitTests.Application.Segment;

public class AddDeviceCommandHandlerTest
{
    private readonly IUnitOfWork _uow;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly AddDeviceCommandHandler _addDeviceCommandHandler;

    public AddDeviceCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _addDeviceCommandHandler = new AddDeviceCommandHandler(_uow, _subscriberRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInvalid()
    {
        //Arrange   
        AddDeviceCommand command = new("test", new("endpoint", "256dh", "auth"), new("Android"), 10);
        _subscriberRepository.FindAsync(Arg.Any<int>()).ReturnsNull();

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<int>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentApplicationErrors.Device.InvalidSubscriber);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInActive()
    {
        //Arrange
        AddDeviceCommand command = new("test", new("endpoint", "256dh", "auth"), new("Android"), 10);
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");
        subscriber.Data.DeActivate();
        _subscriberRepository.FindAsync(Arg.Any<int>()).Returns(subscriber.Data);

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<int>());
        //subscriber.Data.Received().AddDevice(Arg.Any<string>(), Arg.Any<PushManager>(), Arg.Any<ClientMetadata>()); !!??

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.SubscriberIsInActive);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SaveChangesFails()
    {
        //Arrange
        AddDeviceCommand command = new("test", new("endpoint", "256dh", "auth"), new("Android"), 10);
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");
        _subscriberRepository.FindAsync(Arg.Any<int>()).Returns(subscriber.Data);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Error.Exception("SaveChangesError", "SaveChanges Error"));

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<int>());
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    }

    [Fact]
    public async Task Handle_Should_ReturnDeviceId_When_SaveChangesSuccess()
    {
        //Arrange
        AddDeviceCommand command = new("test", new("endpoint", "256dh", "auth"), new("Android"), 10);
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");
        _subscriberRepository.FindAsync(Arg.Any<int>()).Returns(subscriber.Data);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(Arg.Any<int>());
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        subscriber.Data.Devices.Any(x => x.Id == result.Data).Should().BeTrue();
    }
}

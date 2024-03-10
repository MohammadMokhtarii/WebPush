using Core.Application.Common;
using Core.Application.Segment;
using Core.Domain.Exceptions;
using Core.Domain.Segment;

using Services.Common;

namespace Core.UnitTests.Application.Segment.Devices;

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
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(10);
        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.FindAsync(subscriberId).ReturnsNull();

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(subscriberId);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentApplicationErrors.InvalidSubscriber);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInActive()
    {
        //Arrange
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");
        subscriber.DeActivate();

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = subscriber.Id;

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.FindAsync(subscriberId).Returns(subscriber);

        //Act
        var result = async () => await _addDeviceCommandHandler.Handle(command, default);

        //Assert        
        await result.Should().ThrowAsync<SubscriberIsInActiveDomainException>();
        await _subscriberRepository.Received().FindAsync(subscriberId);
        //subscriber.Data.Received().AddDevice(Arg.Any<string>(), Arg.Any<PushManager>(), Arg.Any<ClientMetadata>()); !!??

    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SaveChangesFails()
    {
        //Arrange
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = subscriber.Id;

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.FindAsync(subscriberId).Returns(subscriber);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(UnitOfWorkErrors.SaveChangesError);

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(subscriberId);
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    }

    [Fact]
    public async Task Handle_Should_ReturnDeviceId_When_SaveChangesSuccess()
    {
        //Arrange
        var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");

        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = subscriber.Id;

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.FindAsync(subscriberId).Returns(subscriber);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        //Act
        var result = await _addDeviceCommandHandler.Handle(command, default);

        //Assert
        await _subscriberRepository.Received().FindAsync(subscriberId);
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        subscriber.Devices.Any(x => x.Id.Value == result.Data).Should().BeTrue();
    }
}

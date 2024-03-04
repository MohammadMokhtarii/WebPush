using Core.Application.Common;
using Core.Application.Push;
using Core.Application.Segment;
using Core.Domain.Push;
using Core.Domain.Segment;
using MediatR;
using Services.Common;

namespace Core.UnitTests.Application.Push.Notification;

public class SendNotificationCommandHandlerTest
{
    private readonly IUnitOfWork _uow;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPublisher _publisher;
    private readonly SendNotificationCommandHandler _handler;

    public SendNotificationCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _publisher = Substitute.For<IPublisher>();
        _handler = new SendNotificationCommandHandler(_uow, _notificationRepository, _publisher);
    }

    //[Fact]
    //public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInvalid()
    //{
    //    //Arrange   
    //    string deviceName = "FakeName";
    //    PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
    //    ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
    //    SubscriberId subscriberId = new(10);
    //    AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

    //    _notificationRepository.FindAsync(subscriberId).ReturnsNull();

    //    //Act
    //    var result = await _handler.Handle(command, default);

    //    //Assert
    //    await _notificationRepository.Received().FindAsync(subscriberId);

    //    result.IsFailure.Should().BeTrue();
    //    result.Error.Should().Be(SegmentApplicationErrors.InvalidSubscriber);
    //}


    //[Fact]
    //public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInActive()
    //{
    //    //Arrange
    //    var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");
    //    subscriber.Data.DeActivate();

    //    string deviceName = "FakeName";
    //    PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
    //    ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
    //    SubscriberId subscriberId = subscriber.Data.Id;

    //    AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

    //    _notificationRepository.FindAsync(subscriberId).Returns(subscriber.Data);

    //    //Act
    //    var result = await _handler.Handle(command, default);

    //    //Assert
    //    await _notificationRepository.Received().FindAsync(subscriberId);
    //    //subscriber.Data.Received().AddDevice(Arg.Any<string>(), Arg.Any<PushManager>(), Arg.Any<ClientMetadata>()); !!??

    //    result.IsFailure.Should().BeTrue();
    //    result.Error.Should().Be(SegmentDomainErrors.Subscriber.SubscriberIsInActive);
    //}

    //[Fact]
    //public async Task Handle_Should_ReturnValidationError_When_SaveChangesFails()
    //{
    //    //Arrange
    //    var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");

    //    string deviceName = "FakeName";
    //    PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
    //    ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
    //    SubscriberId subscriberId = subscriber.Data.Id;

    //    AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

    //    _notificationRepository.FindAsync(subscriberId).Returns(subscriber.Data);
    //    _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(UnitOfWorkErrors.SaveChangesError);

    //    //Act
    //    var result = await _handler.Handle(command, default);

    //    //Assert
    //    await _notificationRepository.Received().FindAsync(subscriberId);
    //    await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

    //    result.IsFailure.Should().BeTrue();
    //    result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    //}

    //[Fact]
    //public async Task Handle_Should_ReturnDeviceId_When_SaveChangesSuccess()
    //{
    //    //Arrange
    //    var subscriber = Subscriber.Create("Subscriber Name", "https://subscriber-url.com");

    //    string deviceName = "FakeName";
    //    PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
    //    ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
    //    SubscriberId subscriberId = subscriber.Data.Id;

    //    AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

    //    _notificationRepository.FindAsync(subscriberId).Returns(subscriber.Data);
    //    _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

    //    //Act
    //    var result = await _handler.Handle(command, default);

    //    //Assert
    //    await _notificationRepository.Received().FindAsync(subscriberId);
    //    await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

    //    result.IsSucess.Should().BeTrue();
    //    result.Error.Should().Be(Error.None);
    //    subscriber.Data.Devices.Any(x => x.Id.Value == result.Data).Should().BeTrue();
    //}
}

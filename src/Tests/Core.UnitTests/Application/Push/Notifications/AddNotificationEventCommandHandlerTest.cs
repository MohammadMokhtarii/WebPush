using Core.Application.Common;
using Core.Application.Push;
using Core.Application.Segment;
using Core.Domain.Push;
using Core.Domain.Segment;

using Services.Common;

namespace Core.UnitTests.Application.Push.Notification;

public class AddNotificationEventCommandHandlerTest
{
    private readonly IUnitOfWork _uow;
    private readonly INotificationRepository _notificationRepository;
    private readonly AddNotificationEventCommandHandler _handler;

    public AddNotificationEventCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _handler = new AddNotificationEventCommandHandler(_uow, _notificationRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_NotificationIsInvalid()
    {
        //Arrange   
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;
        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);

        _notificationRepository.FindAsync(notificationId).ReturnsNull();

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _notificationRepository.Received().FindAsync(notificationId);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PushApplicationErrors.InvalidNotification);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SaveChangesFails()
    {
        //Arrange
        DeviceId deviceId = new(10);
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;
        string title = "FakeTitle";
        string message = "FakeBody";
        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);
        Core.Domain.Push.Notification notification = Core.Domain.Push.Notification.Create(deviceId, title, message).Data;

        _notificationRepository.FindAsync(notificationId).Returns(notification);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(UnitOfWorkErrors.SaveChangesError);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _notificationRepository.Received().FindAsync(notificationId);
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    }

    [Fact]
    public async Task Handle_Should_ReturnDeviceId_When_SaveChangesSuccess()
    {
        //Arrange
        DeviceId deviceId = new(10);
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;
        string title = "FakeTitle";
        string message = "FakeBody";
        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);
        Core.Domain.Push.Notification notification = Core.Domain.Push.Notification.Create(deviceId, title, message).Data;
        notification.ChangeStatus(NotificationStatus.Successful, "Fake Description");
        _notificationRepository.FindAsync(notificationId).Returns(notification);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _notificationRepository.Received().FindAsync(notificationId);
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        notification.NotificationEvents.Any(x => x.EventTypeId == notificationEventTypeId).Should().BeTrue();
    }


    [Fact]
    public async Task Handle_Should_AddNoNotificationEvent_When_NotificationStatusIsNotSucces()
    {
        //Arrange
        DeviceId deviceId = new(10);
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;
        string title = "FakeTitle";
        string message = "FakeBody";
        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);
        Core.Domain.Push.Notification notification = Core.Domain.Push.Notification.Create(deviceId, title, message).Data;
        notification.ChangeStatus(NotificationStatus.Failed, "FakeDescription");

        _notificationRepository.FindAsync(notificationId).Returns(notification);
        _uow.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        //Act
        var result = await _handler.Handle(command, default);

        //Assert
        await _notificationRepository.Received().FindAsync(notificationId);
        await _uow.Received().SaveChangesAsync(Arg.Any<CancellationToken>());
        //notification.Data.Received().AddEvent(notificationEventTypeId); !!??

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        notification.NotificationEvents.Any(x => x.EventTypeId == notificationEventTypeId).Should().BeFalse();

    }
}

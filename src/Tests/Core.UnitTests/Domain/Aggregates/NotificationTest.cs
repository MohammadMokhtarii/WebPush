using Core.Domain.Exceptions;
using Core.Domain.Push;
using Core.Domain.Segment;
using NSubstitute.ExceptionExtensions;
using Services.Common;

namespace Core.UnitTests.Domain.Aggregates;

public class NotificationTest
{

    [Fact]
    public void Create_Should_ReturnValidtionError_When_BodyInValid()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = RandomStringGenerator.Generate(200);
        string message = RandomStringGenerator.Generate(100);

        //Act
        var result = () => Notification.Create(deviceId, title, message);

        //Assert
        result.Should().Throw<NotificationPayloadIsInvalidDomainException>();

    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";

        //Act
        var result = () => Notification.Create(deviceId, title, message);

        //Assert
        result.Should().NotThrow<NotificationPayloadIsInvalidDomainException>();
        result().DeviceId.Should().Be(deviceId);
        result().Body.Title.Should().Be(title);
        result().Body.Message.Should().Be(message);
    }


    [Fact]
    public void ChangeStatus_Should_AddNothing_When_NotificationStatusIsAlreadySuccessful()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);
        notification.ChangeStatus(NotificationStatus.Successful, "Description");

        //Act
        notification.ChangeStatus(NotificationStatus.Failed, "Description");

        //Assert

        notification.NotificationActivities.Should().HaveCount(1);
        notification.NotificationEvents.Should().HaveCount(1);
    }


    [Fact]
    public void ChangeStatus_Should_ReturnSuccess_When_NotificationStatusIsSuccessful()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.ChangeStatus(NotificationStatus.Successful, "Description");

        //Assert

        notification.NotificationActivities.Should().HaveCount(1);
        notification.NotificationEvents.Should().HaveCount(1);
    }

    [Fact]
    public void ChangeStatus_Should_AddOnlyNotificationActivity_When_NotificationStatusIsFailed()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.ChangeStatus(NotificationStatus.Failed, "Description");

        //Assert

        notification.NotificationActivities.Should().HaveCount(1);
        notification.NotificationEvents.Should().BeEmpty();
    }


    [Fact]
    public void AddEvent_Should_AddNotificationEvent_When_NotificationStatusIsSucessfull()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);
        notification.ChangeStatus(NotificationStatus.Successful, string.Empty);

        //Act
        notification.AddEvent(NotificationEventType.Sent);

        //Assert

        notification.NotificationEvents.Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public void AddEvent_Should_AddNotificationEvent_When_NotificationStatusIsNotSucessfull()
    {
        //Arrange
        DeviceId deviceId = new(10);
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.AddEvent(NotificationEventType.Sent);

        //Assert

        notification.NotificationEvents.Should().HaveCount(0);
    }
}

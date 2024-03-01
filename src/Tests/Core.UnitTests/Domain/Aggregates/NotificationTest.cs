using Core.Domain.Push;
using Core.Domain.Segment;
using FluentAssertions;
using NSubstitute;
using Services.Common;

namespace Core.UnitTests.Domain.Aggregates;

public class NotificationTest
{

    [Fact]
    public void Create_Should_ReturnValidtionError_When_BodyInValid()
    {
        //Arrange
        int deviceId = 10;
        string title = RandomStringGenerator.Generate(200);
        string message = RandomStringGenerator.Generate(100);

        //Act
        var result = Notification.Create(deviceId, title, message);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBe(Error.None);

    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        int deviceId = 10;
        string title = "test notification";
        string message = "test notification message";

        //Act
        var result = Notification.Create(deviceId, title, message);

        //Assert
        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Data.DeviceId.Should().Be(deviceId);
        result.Data.Body.Title.Should().Be(title);
        result.Data.Body.Message.Should().Be(message);
    }


    [Fact]
    public void ChangeStatus_Should_AddNothing_When_NotificationStatusIsAlreadySuccessful()
    {
        //Arrange
        int deviceId = 10;
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);
        notification.Data.ChangeStatus(NotificationStatus.Successful, "Description");

        //Act
        notification.Data.ChangeStatus(NotificationStatus.Failed, "Description");

        //Assert

        notification.Data.NotificationActivities.Should().HaveCount(1);
        notification.Data.NotificationEvents.Should().HaveCount(1);
    }


    [Fact]
    public void ChangeStatus_Should_ReturnSuccess_When_NotificationStatusIsSuccessful()
    {
        //Arrange
        int deviceId = 10;
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.Data.ChangeStatus(NotificationStatus.Successful, "Description");

        //Assert

        notification.Data.NotificationActivities.Should().HaveCount(1);
        notification.Data.NotificationEvents.Should().HaveCount(1);
    }

    [Fact]
    public void ChangeStatus_Should_AddOnlyNotificationActivity_When_NotificationStatusIsFailed()
    {
        //Arrange
        int deviceId = 10;
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.Data.ChangeStatus(NotificationStatus.Failed, "Description");

        //Assert

        notification.Data.NotificationActivities.Should().HaveCount(1);
        notification.Data.NotificationEvents.Should().BeEmpty();
    }


    [Fact]
    public void AddEvent_Should_AddNotificationEvent()
    {
        //Arrange
        int deviceId = 10;
        string title = "test notification";
        string message = "test notification message";
        var notification = Notification.Create(deviceId, title, message);

        //Act
        notification.Data.AddEvent(NotificationEventType.Sent);

        //Assert

        notification.Data.NotificationEvents.Should().HaveCount(1);
    }
}

using Core.Domain.Push;
using FluentAssertions;
using Services.Common;

namespace Core.UnitTests.Domain.ValueObjects;


public class NotificationPayloadTest
{
    [Theory]
    [InlineData("", "fake message")]
    [InlineData(" ", "fake message")]
    public void Create_Should_ReturnTitleCanNotBeEmpty_When_TitleIsEmpty(string title, string message)
    {

        //Act
        var result = NotificationPayload.Create(title, message);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PushDomainErrors.Notification.NotificationPayload.TitleCanNotBeEmpty);

    }


    [Theory]
    [InlineData("fake message", "")]
    [InlineData("fake message", " ")]
    public void Create_Should_ReturnMessageCanNotBeEmpty_When_MessageIsEmpty(string title, string message)
    {
        //Act
        var result = NotificationPayload.Create(title, message);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PushDomainErrors.Notification.NotificationPayload.MessageCanNotBeEmpty);
    }

    [Fact]
    public void Create_Should_ReturnTitleMaxLengthExcced_When_TitleIsMoreThanMaxlen()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(255);
        string message = RandomStringGenerator.Generate(10);


        //Act
        var result = NotificationPayload.Create(title, message);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PushDomainErrors.Notification.NotificationPayload.TitleMaxLengthExcced);
    }

    [Fact]
    public void Create_Should_ReturnMessageMaxLengthExcced_When_MessageIsIsMoreThanMaxlen()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(10);
        string message = RandomStringGenerator.Generate(550);

        //Act
        var result = NotificationPayload.Create(title, message);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PushDomainErrors.Notification.NotificationPayload.MessageMaxLengthExcced);
    }


    [Fact]
    public void Create_Should_ReturnNotificationPayload_When_ValidInput()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(150);
        string message = RandomStringGenerator.Generate(500);

        //Act
        var result = NotificationPayload.Create(title, message);

        //Assert

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
        result.Data.Title.Should().Be(title);
        result.Data.Message.Should().Be(message);

    }
}


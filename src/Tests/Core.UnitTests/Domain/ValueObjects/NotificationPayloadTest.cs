using Core.Domain.Exceptions;
using Core.Domain.Push;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute.ExceptionExtensions;
using Services.Common;
using System;

namespace Core.UnitTests.Domain.ValueObjects;


public class NotificationPayloadTest
{
    [Theory]
    [InlineData("", "fake message")]
    [InlineData(" ", "fake message")]
    public void Create_Should_ReturnTitleCanNotBeEmpty_When_TitleIsEmpty(string title, string message)
    {

        //Act
        var result = () => NotificationPayload.Create(title, message);

        //Assert
        result.Should().Throw<NotificationPayloadIsInvalidDomainException>();
    }


    [Theory]
    [InlineData("fake message", "")]
    [InlineData("fake message", " ")]
    public void Create_Should_ReturnMessageCanNotBeEmpty_When_MessageIsEmpty(string title, string message)
    {
        //Act
        var result = () => NotificationPayload.Create(title, message);

        //Assert

        result.Should().Throw<NotificationPayloadIsInvalidDomainException>();
    }

    [Fact]
    public void Create_Should_ReturnTitleMaxLengthExcced_When_TitleIsMoreThanMaxlen()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(255);
        string message = RandomStringGenerator.Generate(10);


        //Act
        var result = () => NotificationPayload.Create(title, message);

        //Assert

        result.Should().Throw<NotificationPayloadIsInvalidDomainException>();
    }

    [Fact]
    public void Create_Should_ReturnMessageMaxLengthExcced_When_MessageIsIsMoreThanMaxlen()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(10);
        string message = RandomStringGenerator.Generate(550);

        //Act
        var result = () => NotificationPayload.Create(title, message);

        //Assert

        result.Should().Throw<NotificationPayloadIsInvalidDomainException>();
    }


    [Fact]
    public void Create_Should_ReturnNotificationPayload_When_ValidInput()
    {
        //Arrange
        string title = RandomStringGenerator.Generate(150);
        string message = RandomStringGenerator.Generate(500);

        //Act
        var result = () => NotificationPayload.Create(title, message);

        //Assert

        result.Should().NotThrow<NotificationPayloadIsInvalidDomainException>();
        result().Title.Should().Be(title);
        result().Message.Should().Be(message);

    }
}


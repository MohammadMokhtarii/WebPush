using Core.Application.Common.Resources;
using Core.Application.Push;
using Core.Application.Segment;
using Core.Domain.Push;
using Core.Domain.Segment;
using FluentValidation;
using static Core.Application.Push.SendNotificationCommand;

namespace Core.UnitTests.Application.Push.Notification;

public class SendNotificationCommandValidatorTest
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly SendNotificationCommandValidator _validator;

    public SendNotificationCommandValidatorTest()
    {
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _validator = new SendNotificationCommandValidator(_subscriberRepository);
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_DeviceIdIsEmpty()
    {
        //Arrange
        DeviceId deviceId = new(0);
        SubscriberId subscriberId = new(0);
        string title = "FakeTitle";
        string message = "FakeBody";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.DeviceId) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_DeviceIdIsInValid()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(20);
        string title = "FakeTitle";
        string message = "FakeBody";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(false);


        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _subscriberRepository.Received().DeviceExistsAsync(deviceId, subscriberId, default);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.DeviceId) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_TitleIsEmpty()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(20);
        string title = "";
        string message = "FakeBody";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == $"{nameof(command.Payload)}.{nameof(command.Payload.Title)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NameExceedMaxLength()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(20);
        string title = RandomStringGenerator.Generate(160);
        string message = "FakeBody";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == $"{nameof(command.Payload)}.{nameof(command.Payload.Title)}" && x.ErrorCode == nameof(AppResource.MaxLengthExceeded)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_MessageIsEmpty()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(20);
        string title = "FakeTitle";
        string message = "";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == $"{nameof(command.Payload)}.{nameof(command.Payload.Message)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_MessageExceedMaxLength()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(20);
        string title = "FakeTitle";
        string message = RandomStringGenerator.Generate(501);
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == $"{nameof(command.Payload)}.{nameof(command.Payload.Message)}" && x.ErrorCode == nameof(AppResource.MaxLengthExceeded)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        DeviceId deviceId = new(10);
        SubscriberId subscriberId = new(0);
        string title = "FakeTitle";
        string message = "FakeBody";
        NotificationPayloadDto notificationPayload = new(title, message);

        SendNotificationCommand command = new(deviceId, subscriberId, notificationPayload);

        _subscriberRepository.DeviceExistsAsync(deviceId, subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _subscriberRepository.Received().DeviceExistsAsync(deviceId, subscriberId, default);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

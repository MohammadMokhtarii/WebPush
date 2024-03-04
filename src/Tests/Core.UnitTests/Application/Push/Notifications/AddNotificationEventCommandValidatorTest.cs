using Core.Application.Common.Resources;
using Core.Application.Push;
using Core.Application.Segment;
using Core.Domain.Push;
using Core.Domain.Segment;

namespace Core.UnitTests.Application.Push.Notification;

public class AddNotificationEventCommandValidatorTest
{
    private readonly INotificationRepository _notificationRepository;
    private readonly AddNotificationEventCommandValidator _validator;

    public AddNotificationEventCommandValidatorTest()
    {
        _notificationRepository = Substitute.For<INotificationRepository>();
        _validator = new AddNotificationEventCommandValidator(_notificationRepository);
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NotificationIdIsEmpty()
    {
        //Arrange
        NotificationId notificationId = new(0);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;

        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.NotificationId) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NotificationIdIsInValid()
    {
        //Arrange
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;

        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);

        _notificationRepository.ExistsAsync(notificationId, default).Returns(false);


        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _notificationRepository.Received().ExistsAsync(notificationId, default);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.NotificationId) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NotificationEventTypeIdIsEmpty()
    {
        //Arrange
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = default;

        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);
        _notificationRepository.ExistsAsync(notificationId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _notificationRepository.Received().ExistsAsync(notificationId, default);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.NotificationEventTypeId) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NotificationEventTypeIdIsInvalid()
    {
        //Arrange
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = (NotificationEventType)10;

        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);
        _notificationRepository.ExistsAsync(notificationId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _notificationRepository.Received().ExistsAsync(notificationId, default);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Exists(x => x.PropertyName == nameof(command.NotificationEventTypeId) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        NotificationId notificationId = new(10);
        NotificationEventType notificationEventTypeId = NotificationEventType.Clicked;

        AddNotificationEventCommand command = new(notificationId, notificationEventTypeId);

        _notificationRepository.ExistsAsync(notificationId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _notificationRepository.Received().ExistsAsync(notificationId, default);
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

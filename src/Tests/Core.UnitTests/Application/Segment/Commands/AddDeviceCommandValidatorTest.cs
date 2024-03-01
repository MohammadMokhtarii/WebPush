using Core.Application.Common.Resources;
using Core.Application.Segment;
using Core.Domain.Segment;
using FluentAssertions;
using NSubstitute;

namespace Core.UnitTests.Application.Segment;

public class AddDeviceCommandValidatorTest
{
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly AddDeviceCommandValidator _validator;

    public AddDeviceCommandValidatorTest()
    {
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _validator = new AddDeviceCommandValidator(_subscriberRepository);
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NameIsEmpty()
    {
        //Arrange
        AddDeviceCommand command = new("", new("endpoint", "256dh", "auth"), new("Android"), 10);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Name) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NameExceedMaxLength()
    {
        //Arrange
        AddDeviceCommand command = new(RandomStringGenerator.Generate(100), new("endpoint", "256dh", "auth"), new("Android"), 10);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Name) && x.ErrorCode == nameof(AppResource.MaxLengthExceeded)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_SubscriberIdIsEmpty()
    {
        //Arrange
        AddDeviceCommand command = new("fakeName", new("endpoint", "256dh", "auth"), new("Android"), 0);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.SubscriberId) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_SubscriberIdIsInvalid()
    {
        //Arrange
        AddDeviceCommand command = new("fakeName", new("endpoint", "256dh", "auth"), new("Android"), 12);
        _subscriberRepository.ExistsAsync(12, default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.SubscriberId) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        AddDeviceCommand command = new("fakeName", new("endpoint", "256dh", "auth"), new("Android"), 12);

        _subscriberRepository.ExistsAsync(12, default).Returns(true);
        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

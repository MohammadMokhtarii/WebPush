using Core.Application.Common.Resources;
using Core.Application.Segment;
using Core.Domain.Segment;
using FluentAssertions;
using NSubstitute;

namespace Core.UnitTests.Application.Segment.Devices;

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
        string deviceName = "";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(10);
        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

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
        string deviceName = RandomStringGenerator.Generate(100);
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(10);
        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

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
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(0);

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

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
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(12);

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.ExistsAsync(subscriberId, default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.SubscriberId) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_PushManagerIsInvalid()
    {
        //Arrange
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("", "", "");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(12);

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.ExistsAsync(subscriberId, default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == $"{nameof(command.PushManager)}.{nameof(command.PushManager.Endpoint)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
        result.Errors.Any(x => x.PropertyName == $"{nameof(command.PushManager)}.{nameof(command.PushManager.P256DH)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
        result.Errors.Any(x => x.PropertyName == $"{nameof(command.PushManager)}.{nameof(command.PushManager.Auth)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_ClientMetadataIsInvalid()
    {
        //Arrange
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("");
        SubscriberId subscriberId = new(12);

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.ExistsAsync(subscriberId, default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == $"{nameof(command.ClientMetadata)}.{nameof(command.ClientMetadata.OS)}" && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_InputIsValid()
    {
        //Arrange
        string deviceName = "FakeName";
        PushManager devicePushManager = PushManager.Create("Endpoint", "P256", "Auth");
        ClientMetadata deviceClientMetadata = ClientMetadata.Create("Android");
        SubscriberId subscriberId = new(12);

        AddDeviceCommand command = new(deviceName, devicePushManager, deviceClientMetadata, subscriberId);

        _subscriberRepository.ExistsAsync(subscriberId, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

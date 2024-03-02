using Core.Application.Common.Resources;
using Core.Application.Segment;
using Core.Domain.Segment;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace Core.UnitTests.Application.Segment.Subscribers;

public class AddSubscriberCommandValidatorTest
{

    private readonly ISubscriberRepository _subscriberRepository;
    private readonly AddSubscriberCommandValidator _validator;

    public AddSubscriberCommandValidatorTest()
    {
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _validator = new(_subscriberRepository);
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NameIsEmpty()
    {
        //Arrange
        string subscriberName = "";
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);

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
        string subscriberName = RandomStringGenerator.Generate(100);
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Name) && x.ErrorCode == nameof(AppResource.MaxLengthExceeded)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_NameIsDuplicate()
    {
        //Arrange
        string subscriberName = RandomStringGenerator.Generate(100);
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);
        _subscriberRepository.IsUniqueAsync(subscriberName, default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Name) && x.ErrorCode == nameof(AppResource.MustBeUnique)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_NameIsValid()
    {
        //Arrange
        string subscriberName = RandomStringGenerator.Generate(50);
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);
        _subscriberRepository.IsUniqueAsync(subscriberName, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }


    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_UrlIsEmpty()
    {
        //Arrange
        string subscriberName = "FakeName";
        string url = string.Empty;
        AddSubscriberCommand command = new(subscriberName, url);
        _subscriberRepository.IsUniqueAsync(subscriberName, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.WebsiteUrl) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_UrlExceedMaxLength()
    {
        //Arrange
        string subscriberName = "FakeName";
        string url = RandomStringGenerator.Generate(100);
        AddSubscriberCommand command = new(subscriberName, url);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.WebsiteUrl) && x.ErrorCode == nameof(AppResource.MaxLengthExceeded)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_UrlIsInValid()
    {
        //Arrange
        string subscriberName = RandomStringGenerator.Generate(20);
        string url = "http://example@example.com";
        AddSubscriberCommand command = new(subscriberName, url);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.WebsiteUrl) && x.ErrorCode == nameof(AppResource.MustBeValid)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_UrlIsValid()
    {
        //Arrange
        string subscriberName = RandomStringGenerator.Generate(50);
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);
        _subscriberRepository.IsUniqueAsync(subscriberName, default).Returns(true);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

}

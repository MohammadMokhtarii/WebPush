using Core.Application.Common.Resources;
using Core.Application.Segment;
using Core.Domain.Segment;

namespace Core.UnitTests.Application.Segment.Subscribers;

public class DeActivateSubscriberCommandValidatorTest
{

    private readonly ISubscriberRepository _subscriberRepository;
    private readonly DeActivateSubscriberCommandValidator _validator;

    public DeActivateSubscriberCommandValidatorTest()
    {
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _validator = new(_subscriberRepository);
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_IdIsEmpty()
    {
        //Arrange
        int id = 0;
        DeActivateSubscriberCommand command = new(new SubscriberId(id));

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Id) && x.ErrorCode == nameof(AppResource.NotEmpty)).Should().BeTrue();
    }

    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_IdIsInvalid()
    {
        //Arrange
        int id = 10;
        DeActivateSubscriberCommand command = new(new SubscriberId(id));
        _subscriberRepository.ExistsAsync(Arg.Any<SubscriberId>(), default).Returns(false);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _subscriberRepository.Received().ExistsAsync(Arg.Any<SubscriberId>(), default);
        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Id) && x.ErrorCode == nameof(AppResource.NotFound)).Should().BeTrue();
    }


    [Fact]
    public async Task Validate_Should_ReturnValidationError_When_IdIsInActive()
    {
        //Arrange
        int id = 10;
        string subscriberName = "FakeName";
        string url = "https://example.com";
        DeActivateSubscriberCommand command = new(new SubscriberId(id));
        Subscriber subscriber = Subscriber.Create(subscriberName, url);
        subscriber.DeActivate();
        _subscriberRepository.ExistsAsync(Arg.Any<SubscriberId>(), default).Returns(true);
        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>(), default).Returns(subscriber);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _subscriberRepository.Received().ExistsAsync(Arg.Any<SubscriberId>(), default);
        await _subscriberRepository.Received().FindAsync(Arg.Any<SubscriberId>(), default);

        result.IsValid.Should().BeFalse();
        result.Errors.Any(x => x.PropertyName == nameof(command.Id) && x.ErrorCode == nameof(AppResource.MustBeActive)).Should().BeTrue();
    }
    [Fact]
    public async Task Validate_Should_ReturnSuccess_When_SubscriberIsValid()
    {
        //Arrange
        int id = 10;
        string subscriberName = "FakeName";
        string url = "https://example.com";
        DeActivateSubscriberCommand command = new(new SubscriberId(id));
        Subscriber subscriber = Subscriber.Create(subscriberName, url);

        _subscriberRepository.ExistsAsync(Arg.Any<SubscriberId>(), default).Returns(true);
        _subscriberRepository.FindAsync(Arg.Any<SubscriberId>(), default).Returns(subscriber);

        //Act
        var result = await _validator.ValidateAsync(command, default);

        //Assert
        await _subscriberRepository.Received().ExistsAsync(Arg.Any<SubscriberId>(), default);
        await _subscriberRepository.Received().FindAsync(Arg.Any<SubscriberId>(), default);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

}

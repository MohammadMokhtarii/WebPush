using Core.Application.Common;
using Core.Application.Segment;
using Core.Domain.Segment;
using Services.Common;

namespace Core.UnitTests.Application.Segment.Subscribers;

public class AddSubscriberCommandHandlerTest
{
    private readonly IUnitOfWork _uow;
    private readonly ISubscriberRepository _subscriberRepository;
    private readonly AddSubscriberCommandHandler _handler;

    public AddSubscriberCommandHandlerTest()
    {
        _uow = Substitute.For<IUnitOfWork>();
        _subscriberRepository = Substitute.For<ISubscriberRepository>();
        _handler = new(_uow, _subscriberRepository);
    }


    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SubscriberIsInvalid()
    {
        //Arrange   
        string subscriberName = "";
        string url = "http://example.com";
        AddSubscriberCommand command = new(subscriberName, url);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SegmentDomainErrors.Subscriber.WebsiteUrl.InvalidUrl);
    }

    [Fact]
    public async Task Handle_Should_ReturnValidationError_When_SaveChangesFailed()
    {
        //Arrange   
        string subscriberName = "";
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);
        _uow.SaveChangesAsync(default).Returns(UnitOfWorkErrors.SaveChangesError);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UnitOfWorkErrors.SaveChangesError);
    }

    [Fact]
    public async Task Handle_Should_ReturnSubscriberId_When_SaveChangesSucessed()
    {
        //Arrange   
        string subscriberName = "";
        string url = "https://example.com";
        AddSubscriberCommand command = new(subscriberName, url);
        _uow.SaveChangesAsync(default).Returns(Result.Success);

        //Act
        var result = await _handler.Handle(command, default);

        //Assert

        result.IsSucess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }
}

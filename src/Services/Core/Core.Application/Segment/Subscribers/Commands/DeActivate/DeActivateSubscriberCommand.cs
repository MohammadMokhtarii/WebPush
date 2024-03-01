namespace Core.Application.Segment;
public record DeActivateSubscriberCommand(int Id) : IRequest<Result>;
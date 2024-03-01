using Core.Domain.Segment;

namespace Core.Application.Segment;
public record DeActivateSubscriberCommand(SubscriberId Id) : IRequest<Result>;
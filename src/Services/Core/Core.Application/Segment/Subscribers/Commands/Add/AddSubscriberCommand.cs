namespace Core.Application.Segment;
public record AddSubscriberCommand(string Name, string WebsiteUrl) : IRequest<Result<int>>;


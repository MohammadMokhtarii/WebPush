using Core.Domain.Segment;

namespace Core.Application.Common;

public interface IWebPushAdapter
{
    Task<Result> SendAsync(PushManager item, string message, CancellationToken cancellationToken = default);
}


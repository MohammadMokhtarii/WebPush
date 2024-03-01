using static Core.Application.Push.SendNotificationCommand;

namespace Core.Application.Push;
public record SendNotificationCommand(int DeviceId, int SubscriberId, NotificationPayloadDto Payload) : IRequest<Result<int>>
{
    public record NotificationPayloadDto(string Title, string Message);
}


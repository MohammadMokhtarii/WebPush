using Core.Domain.Segment;
using static Core.Application.Push.SendNotificationCommand;

namespace Core.Application.Push;
public record SendNotificationCommand(DeviceId DeviceId, SubscriberId SubscriberId, NotificationPayloadDto Payload) : IRequest<Result<int>>
{
    public record SendNotificationDto(int DeviceId, int SubscriberId, NotificationPayloadDto Payload);
    public record NotificationPayloadDto(string Title, string Message);
}


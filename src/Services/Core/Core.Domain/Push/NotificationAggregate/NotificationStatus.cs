namespace Core.Domain.Push;

public enum NotificationStatus : byte
{
    Pending = 1,
    Failed = 2,
    Sent = 3,
    Successful = 4,
}
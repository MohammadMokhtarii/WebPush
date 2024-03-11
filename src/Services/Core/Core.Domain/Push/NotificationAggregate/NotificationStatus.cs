namespace Core.Domain.Push;

public enum NotificationStatus : byte
{
    Pending = 1,
    Sent = 2,
    Failed = 3,
    Successful = 4,
}
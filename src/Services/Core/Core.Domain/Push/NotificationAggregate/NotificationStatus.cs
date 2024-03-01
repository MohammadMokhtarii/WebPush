namespace Core.Domain.Push;

public enum NotificationStatus : byte
{
    Pending = 1,
    Failed = 2,
    Successful = 3,
}
namespace Core.Domain.Push;

public enum NotificationEventType : byte
{
    Sent = 1,
    Delivered = 2,
    Clicked = 3
}
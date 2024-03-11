namespace Core.Domain.Exceptions;


public sealed class NotificationPayloadIsInvalidDomainException(string message) : DomainException(message)
{
}
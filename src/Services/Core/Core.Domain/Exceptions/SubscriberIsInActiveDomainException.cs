namespace Core.Domain.Exceptions;

public sealed class SubscriberIsInActiveDomainException(string message) : DomainException(message)
{
}
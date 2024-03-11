namespace Core.Domain.Exceptions;


public sealed class SubscriberUrlIsInvalidDomainException(string message) : DomainException(message)
{
}
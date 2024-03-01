namespace Core.Domain;

public abstract class Entity
{
    protected Entity() => _domainEvents = [];

    private readonly List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void RemoveDomainEvent(IDomainEvent @event) => _domainEvents.Remove(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

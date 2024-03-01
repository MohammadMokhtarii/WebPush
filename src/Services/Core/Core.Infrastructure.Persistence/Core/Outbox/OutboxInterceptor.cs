using Core.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Core.Infrastructure.Persistence.Core;

public sealed class OutboxInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        StoreDomainEvents(eventData.Context!);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        StoreDomainEvents(eventData.Context!);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public static void StoreDomainEvents(DbContext context)
    {
        var entities = context.ChangeTracker.Entries<Entity>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);
        var domainEvents = entities.SelectMany(x =>
        {
            var events = x.Entity.DomainEvents;
            x.Entity.ClearDomainEvents();
            return events;

        }).Select(domainEvent => new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().Name,
            Content = Newtonsoft.Json.JsonConvert.SerializeObject(domainEvent, new Newtonsoft.Json.JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            })
        }).ToList();
        if (domainEvents.Count != 0)
            context.Set<OutboxMessage>().AddRange(domainEvents);
    }
}

namespace Core.Infrastructure.Persistence.Core;

public sealed class OutboxMessage
{
    public OutboxMessage()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string Content { get; set; } = default!;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}

namespace EventPatterns.Example.Core.Abstractions;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

    public Guid EventId { get; set; } = Guid.NewGuid();
}

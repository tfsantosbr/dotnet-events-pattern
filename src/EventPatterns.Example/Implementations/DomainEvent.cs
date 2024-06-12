using EventPatterns.Example.Abstractions;

namespace EventPatterns.Example.Implementations;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

    public Guid EventId { get; set; } = Guid.NewGuid();
}

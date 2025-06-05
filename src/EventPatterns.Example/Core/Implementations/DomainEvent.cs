using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

    public Guid EventId { get; set; } = Guid.NewGuid();
}

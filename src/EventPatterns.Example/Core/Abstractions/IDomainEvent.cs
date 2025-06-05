namespace EventPatterns.Example.Core.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}

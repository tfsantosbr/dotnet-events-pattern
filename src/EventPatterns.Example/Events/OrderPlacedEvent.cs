using EventPatterns.Example.Implementations;

namespace EventPatterns.Example.Events;

public record OrderPlacedEvent(Guid OrderId) : DomainEvent;

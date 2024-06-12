using EventPatterns.Example.Implementations;

namespace EventPatterns.Example.Events;

public record UserCreatedEvent(Guid UserId, string UserName) : DomainEvent;

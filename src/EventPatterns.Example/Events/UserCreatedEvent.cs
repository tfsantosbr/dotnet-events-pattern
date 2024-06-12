using EventPatterns.Example.Implementations;
using MediatR;

namespace EventPatterns.Example.Events;

public record UserCreatedEvent(Guid UserId) : DomainEvent, INotification;

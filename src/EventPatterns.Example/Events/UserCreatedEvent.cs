using EventPatterns.Example.Core.Implementations;
using MediatR;

namespace EventPatterns.Example.Events;

public record UserCreatedEvent(Guid UserId) : DomainEvent, INotification;

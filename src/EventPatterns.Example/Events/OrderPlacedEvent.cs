using EventPatterns.Example.Implementations;
using MediatR;

namespace EventPatterns.Example.Events;

public record OrderPlacedEvent(Guid OrderId) : DomainEvent, INotification;

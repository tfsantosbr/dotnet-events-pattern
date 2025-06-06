using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;

public record DomainEventHandlerExecutor(
    object HandlerInstance, Func<IDomainEvent, CancellationToken, Task> HandlerCallback);
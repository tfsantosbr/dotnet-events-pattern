using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;

public abstract class DomainEventHandlerWrapper
{
    public abstract Task Handle(IDomainEvent domainEvent, IServiceProvider serviceProvider,
        Func<IEnumerable<DomainEventHandlerExecutor>, IDomainEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken);
}

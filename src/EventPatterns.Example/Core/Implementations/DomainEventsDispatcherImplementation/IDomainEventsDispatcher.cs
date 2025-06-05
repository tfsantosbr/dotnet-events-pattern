using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations.DomainEventsDispatcherImplementation;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvents, CancellationToken cancellationToken = default);
}

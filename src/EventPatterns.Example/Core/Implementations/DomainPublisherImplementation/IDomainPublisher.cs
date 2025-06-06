using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;

public interface IDomainPublisher
{
    Task Publish<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent;
}

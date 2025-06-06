using System.Collections.Concurrent;
using EventPatterns.Example.Core.Abstractions;

namespace EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;

public class DomainPublisher(IServiceProvider serviceProvider) : IDomainPublisher
{
    private static readonly ConcurrentDictionary<Type, DomainEventHandlerWrapper> _domainEventHandlers = new();

    public Task Publish<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var handler = _domainEventHandlers.GetOrAdd(domainEvent.GetType(), static domainEventType =>
        {
            var wrapperType = typeof(DomainEventHandlerWrapperImpl<>).MakeGenericType(domainEventType);
            var wrapper = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException($"Could not create wrapper for type {domainEventType}");
            return (DomainEventHandlerWrapper)wrapper;
        });

        return handler.Handle(domainEvent, serviceProvider, PublishCore, cancellationToken);
    }

    protected static async Task PublishCore(
        IEnumerable<DomainEventHandlerExecutor> handlerExecutors,
        IDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        foreach (var handler in handlerExecutors)
        {
            await handler.HandlerCallback(domainEvent, cancellationToken).ConfigureAwait(false);
        }
    }
}

using EventPatterns.Example.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;

public class DomainEventHandlerWrapperImpl<TDomainEvent> : DomainEventHandlerWrapper
    where TDomainEvent : IDomainEvent
{
    public override Task Handle(IDomainEvent domainEvent, IServiceProvider serviceProvider,
        Func<IEnumerable<DomainEventHandlerExecutor>, IDomainEvent, CancellationToken, Task> publish,
        CancellationToken cancellationToken)
    {
        var handlers = serviceProvider
            .GetServices<IDomainEventHandler<TDomainEvent>>()
            .Select(static handler => new DomainEventHandlerExecutor(
                HandlerInstance: handler,
                HandlerCallback: (eventToHandle, token) =>
                    handler.HandleAsync((TDomainEvent)eventToHandle, token)
                ));

        return publish(handlers, domainEvent, cancellationToken);
    }
}

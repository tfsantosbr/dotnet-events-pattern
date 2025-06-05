using System.Collections.Concurrent;
using EventPatterns.Example.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventPatterns.Example.Core.Implementations.DomainEventsDispatcherImplementation;

internal sealed class DomainEventsDispatcher(IServiceProvider serviceProvider) : IDomainEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        Type domainEventType = domainEvent.GetType();
        Type handlerType = HandlerTypeDictionary.GetOrAdd(domainEventType, et => typeof(IDomainEventHandler<>).MakeGenericType(et));

        IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (object? handler in handlers)
        {
            if (handler is null)
            {
                continue;
            }

            var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);

            await handlerWrapper.Handle(domainEvent, cancellationToken);
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(domainEventType, et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    private sealed class HandlerWrapper<TEvent>(object handler) : HandlerWrapper where TEvent : IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _handler = (IDomainEventHandler<TEvent>)handler;

        public override async Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            await _handler.HandleAsync((TEvent)domainEvent, cancellationToken);
        }
    }
}

using EventPatterns.Example.Abstractions;

namespace EventPatterns.Example.Implementations;

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<Type, List<Func<IDomainEvent, CancellationToken, Task>>> _handlersDictionary = [];

    public async Task DispatchAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken) 
        where TEvent : IDomainEvent
    {
        var eventType = domainEvent.GetType();

        if (!_handlersDictionary.TryGetValue(eventType, out var handlers))
            return;

        await Task.WhenAll(handlers.Select(handler => handler(domainEvent, cancellationToken)));   
    }

    public void Register<TEvent>(Func<TEvent, CancellationToken, Task> handler) 
        where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);

        if (!_handlersDictionary.TryGetValue(eventType, out var handlers))
        {
            handlers = [];
            _handlersDictionary[eventType] = handlers;
        }

        handlers.Add((domainEvent, cancellationToken) => handler((TEvent)domainEvent, cancellationToken));
    }
}

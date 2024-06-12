namespace EventPatterns.Example.Abstractions;

public interface IEventDispatcher
{
    void Register<TEvent>(Func<TEvent, CancellationToken, Task> handler) where TEvent : IDomainEvent;
    Task DispatchAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken) where TEvent : IDomainEvent;
}

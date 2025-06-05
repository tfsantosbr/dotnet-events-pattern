namespace EventPatterns.Example.Core.Abstractions;

public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}

using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using MediatR;

namespace EventPatterns.Example.Handlers;

public class UserCreatedHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"User with id '{userCreatedEvent.UserId}' has been created");

        return Task.CompletedTask;
    }
}

public class UserCreatedMediatrHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User with id '{userCreatedEvent.UserId}' has been created");

        return Task.CompletedTask;
    }
}
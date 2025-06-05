using EventPatterns.Example.Core.Abstractions;
using MediatR;

namespace EventPatterns.Example.Events;

public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"User with id '{userCreatedEvent.UserId}' has been created");

        return Task.CompletedTask;
    }
}

public class UserCreatedEventMediatrHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User with id '{userCreatedEvent.UserId}' has been created");

        return Task.CompletedTask;
    }
}
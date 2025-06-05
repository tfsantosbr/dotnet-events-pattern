using EventPatterns.Example.Core.Abstractions;
using MediatR;

namespace EventPatterns.Example.Events;

public class UserCreatedNotificationHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Sending notification for user creation with ID: '{userCreatedEvent.UserId}'");

        return Task.CompletedTask;
    }
}

public class UserCreatedNotificationMediatrHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending notification for user creation with ID: '{userCreatedEvent.UserId}'");

        return Task.CompletedTask;
    }
}

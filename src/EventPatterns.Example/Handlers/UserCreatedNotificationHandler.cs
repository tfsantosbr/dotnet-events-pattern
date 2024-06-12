using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;

namespace EventPatterns.Example.Handlers;

public class UserCreatedNotificationHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Sending notification for user creation with ID: '{userCreatedEvent.UserId}'");
        
        return Task.CompletedTask;
    }
}

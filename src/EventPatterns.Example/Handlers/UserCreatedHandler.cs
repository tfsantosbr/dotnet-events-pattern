using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;

namespace EventPatterns.Example.Handlers;

public class UserCreatedHandler : IEventHandler<UserCreatedEvent>
{
    public Task HandleAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"User with id '{userCreatedEvent.UserId}' and name '{userCreatedEvent.UserName}' has been created");

        return Task.CompletedTask;
    }
}

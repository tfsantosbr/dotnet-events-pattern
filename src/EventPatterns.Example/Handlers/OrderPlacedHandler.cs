using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;

namespace EventPatterns.Example.Handlers;

public class OrderPlacedHandler : IEventHandler<OrderPlacedEvent>
{
    public Task HandleAsync(OrderPlacedEvent orderPlacedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Order placed with ID: {orderPlacedEvent.OrderId}");
        
        return Task.CompletedTask;
    }
}

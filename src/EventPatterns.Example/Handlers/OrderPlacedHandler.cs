using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using MediatR;

namespace EventPatterns.Example.Handlers;

public class OrderPlacedHandler : IEventHandler<OrderPlacedEvent>
{
    public Task HandleAsync(OrderPlacedEvent orderPlacedEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Order placed with ID: {orderPlacedEvent.OrderId}");
        
        return Task.CompletedTask;
    }
}


public class OrderPlacedMediatrHandler : INotificationHandler<OrderPlacedEvent>
{
    public Task Handle(OrderPlacedEvent orderPlacedEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Order placed with ID: {orderPlacedEvent.OrderId}");
        
        return Task.CompletedTask;
    }
}

using BenchmarkDotNet.Attributes;
using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using EventPatterns.Example.Extensions;
using EventPatterns.Example.Handlers;
using EventPatterns.Example.Implementations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventPatterns.Example.Benchmarking;

[MemoryDiagnoser(true)]
public class EventPatternsBenchmark
{
    private IEventDispatcher _eventDispatcher = null!;
    private IPublisher _publisher = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Manual Handlers

        services.AddTransient<IEventHandler<UserCreatedEvent>, UserCreatedHandler>();
        services.AddTransient<IEventHandler<UserCreatedEvent>, UserCreatedNotificationHandler>();
        services.AddTransient<IEventHandler<OrderPlacedEvent>, OrderPlacedHandler>();

        services.AddSingleton<IEventDispatcher>(serviceProvider =>
        {
            var dispatcher = new EventDispatcher();

            dispatcher.RegisterEventHandlers<UserCreatedEvent>(serviceProvider);
            dispatcher.RegisterEventHandlers<OrderPlacedEvent>(serviceProvider);

            return dispatcher;
        });

        // Mediatr

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UserCreatedEvent>());

        var serviceProvider = services.BuildServiceProvider();

        _eventDispatcher = serviceProvider.GetRequiredService<IEventDispatcher>();
        _publisher = serviceProvider.GetRequiredService<IPublisher>();
    }

    [Benchmark]
    public async Task DomainEventsWithoutMediatr()
    {
        var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
        await _eventDispatcher.DispatchAsync(userCreatedEvent, CancellationToken.None);

        var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
        await _eventDispatcher.DispatchAsync(orderPlacedEvent, CancellationToken.None);
    }

    [Benchmark]
    public async Task DomainEventsWithtMediatr()
    {
        var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
        await _publisher.Publish(userCreatedEvent);

        var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
        await _publisher.Publish(orderPlacedEvent);
    }
}

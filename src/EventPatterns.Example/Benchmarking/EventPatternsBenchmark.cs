using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using EventPatterns.Example.Extensions;
using EventPatterns.Example.Handlers;
using EventPatterns.Example.Implementations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventPatterns.Example.Benchmarking;

[MemoryDiagnoser(true)]
[SimpleJob(RuntimeMoniker.Net90)]
[GcServer(true)]
[GcConcurrent(true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
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

    [Params(100, 50, 1000)]
    public int EventCount { get; set; }

    [Benchmark(Baseline = true)]
    public async Task DomainEventsWithoutMediatr()
    {
        for (int i = 0; i < EventCount; i++)
        {
            var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
            await _eventDispatcher.DispatchAsync(userCreatedEvent, CancellationToken.None);

            var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
            await _eventDispatcher.DispatchAsync(orderPlacedEvent, CancellationToken.None);
        }
    }

    [Benchmark]
    public async Task DomainEventsWithMediatr()
    {
        for (int i = 0; i < EventCount; i++)
        {
            var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
            await _publisher.Publish(userCreatedEvent);

            var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
            await _publisher.Publish(orderPlacedEvent);
        }
    }

    [Benchmark]
    public async Task BatchEventsWithoutMediatr()
    {
        var events = new List<DomainEvent>();

        for (int i = 0; i < EventCount; i++)
        {
            events.Add(new UserCreatedEvent(Guid.NewGuid()));
            events.Add(new OrderPlacedEvent(Guid.NewGuid()));
        }

        foreach (var @event in events)
        {
            await _eventDispatcher.DispatchAsync(@event, CancellationToken.None);
        }
    }

    [Benchmark]
    public async Task BatchEventsWithMediatr()
    {
        var events = new List<DomainEvent>();

        for (int i = 0; i < EventCount; i++)
        {
            events.Add(new UserCreatedEvent(Guid.NewGuid()));
            events.Add(new OrderPlacedEvent(Guid.NewGuid()));
        }

        foreach (var @event in events)
        {
            await _publisher.Publish(@event);
        }
    }
}

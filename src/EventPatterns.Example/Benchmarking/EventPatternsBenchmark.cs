using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using EventPatterns.Example.Core.Implementations.DomainEventsDispatcherImplementation;
using EventPatterns.Example.Events;
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
    private IDomainEventsDispatcher _domainEventsDispatcher = null!;
    private IPublisher _publisher = null!;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        var assembly = typeof(Program).Assembly;

        // Add Domain Handlers
        services.AddDomainHandlersFromAssembly(assembly);

        // Add Mediatr Handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        var serviceProvider = services.BuildServiceProvider();
        _domainEventsDispatcher = serviceProvider.GetRequiredService<IDomainEventsDispatcher>();
        _publisher = serviceProvider.GetRequiredService<IPublisher>();

    }

    [Params(10000, 100000, 1000000)]
    public int EventCount { get; set; }

    [Benchmark(Baseline = true)]
    public async Task MediatrPublisher()
    {
        for (int i = 0; i < EventCount; i++)
        {
            await _publisher.Publish(new SampleDomainEvent250());
            await _publisher.Publish(new SampleDomainEvent420());
        }
    }

    [Benchmark]
    public async Task DomainEventsDispatcher()
    {
        for (int i = 0; i < EventCount; i++)
        {
            await _domainEventsDispatcher.DispatchAsync(new SampleDomainEvent250());
            await _domainEventsDispatcher.DispatchAsync(new SampleDomainEvent420());
        }
    }
}

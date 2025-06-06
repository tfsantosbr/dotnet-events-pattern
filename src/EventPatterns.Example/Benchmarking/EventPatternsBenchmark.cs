using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using EventPatterns.Example.Core.Extensions;
using EventPatterns.Example.Core.Implementations.DomainPublisherImplementation;
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
    private IPublisher _mediatrPublisher = null!;
    private IDomainPublisher _domainPublisher = null!;

    [Params(10000, 100000, 1000000)]
    public int EventCount { get; set; }

    [GlobalSetup(Target = nameof(MediatrPublisher))]
    public void SetupMediatrPublisher()
    {
        var services = new ServiceCollection();
        var assembly = typeof(Program).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        var provider = services.BuildServiceProvider();
        _mediatrPublisher = provider.GetRequiredService<IPublisher>();
    }

    [GlobalSetup(Target = nameof(DomainPublisher))]
    public void SetupDomainPublisher()
    {
        var services = new ServiceCollection();
        var assembly = typeof(Program).Assembly;

        services.AddDomainHandlersFromAssembly(assembly);
        services.AddTransient<IDomainPublisher, DomainPublisher>();

        var provider = services.BuildServiceProvider();
        _domainPublisher = provider.GetRequiredService<IDomainPublisher>();
    }

    [Benchmark(Baseline = true)]
    public async Task MediatrPublisher()
    {
        for (int i = 0; i < EventCount; i++)
        {
            await _mediatrPublisher.Publish(new SampleDomainEvent250());
            await _mediatrPublisher.Publish(new SampleDomainEvent420());
        }
    }

    [Benchmark]
    public async Task DomainPublisher()
    {
        for (int i = 0; i < EventCount; i++)
        {
            await _domainPublisher.Publish(new SampleDomainEvent250());
            await _domainPublisher.Publish(new SampleDomainEvent420());
        }
    }
}

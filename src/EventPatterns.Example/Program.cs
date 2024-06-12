using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using EventPatterns.Example.Extensions;
using EventPatterns.Example.Handlers;
using EventPatterns.Example.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole());

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

var app = services.BuildServiceProvider();

var dispatcher = app.GetRequiredService<IEventDispatcher>();

var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
await dispatcher.DispatchAsync(userCreatedEvent, CancellationToken.None);

var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
await dispatcher.DispatchAsync(orderPlacedEvent, CancellationToken.None);
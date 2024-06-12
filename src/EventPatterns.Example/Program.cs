using EventPatterns.Example.Abstractions;
using EventPatterns.Example.Events;
using EventPatterns.Example.Handlers;
using EventPatterns.Example.Implementations;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<IEventHandler<UserCreatedEvent>, UserCreatedHandler>();
services.AddTransient<IEventHandler<UserCreatedEvent>, UserCreatedNotificationHandler>();
services.AddTransient<IEventHandler<OrderPlacedEvent>, OrderPlacedHandler>();

services.AddSingleton<IEventDispatcher>(serviceProvider =>
{
    var dispatcher = new EventDispatcher();

    var userCreatedHandlers = serviceProvider.GetServices<IEventHandler<UserCreatedEvent>>();
    foreach (var handler in userCreatedHandlers)
    {
        dispatcher.Register<UserCreatedEvent>(handler.HandleAsync);
    }

    var orderPlacedHandlers = serviceProvider.GetServices<IEventHandler<OrderPlacedEvent>>();
    foreach (var handler in orderPlacedHandlers)
    {
        dispatcher.Register<OrderPlacedEvent>(handler.HandleAsync);
    }

    return dispatcher;
});

var app = services.BuildServiceProvider();

var dispatcher = app.GetRequiredService<IEventDispatcher>();

var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid(), "Tiago Santos");
await dispatcher.DispatchAsync(userCreatedEvent, CancellationToken.None);

var orderPlacedEvent = new OrderPlacedEvent(Guid.NewGuid());
await dispatcher.DispatchAsync(orderPlacedEvent, CancellationToken.None);
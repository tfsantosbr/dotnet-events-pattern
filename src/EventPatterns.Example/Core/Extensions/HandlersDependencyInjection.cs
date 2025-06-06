using System.Reflection;
using EventPatterns.Example.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventPatterns.Example.Core.Extensions;

public static class HandlersDependencyInjection
{
    public static IServiceCollection AddDomainHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<Type> types = assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsInterface);

        foreach (Type type in types)
        {
            Type[] typeInterfaces = type.GetInterfaces();

            foreach (Type typeInterface in typeInterfaces)
            {
                if (typeInterface.IsGenericType &&
                    typeInterface.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                {
                    services.AddTransient(typeInterface, type);
                }
            }
        }

        return services;
    }
}
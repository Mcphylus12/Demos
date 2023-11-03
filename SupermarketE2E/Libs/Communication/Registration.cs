using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Communication.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Communication;

public static class Registration
{
    public static void AddCommunication(this IServiceCollection services, params Type[] markerTypes)
    {
        services.AddHttpClient();
        services.AddTransient<ISender, Sender>();

        IEnumerable<KeyValuePair<string, Type>> requestMap = new Dictionary<string, Type>();
        IEnumerable<KeyValuePair<string, Type>> messageMap = new Dictionary<string, Type>();
        var assemblies = markerTypes.Select(mt => mt.Assembly);
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            requestMap = requestMap.Concat(LoadRequestHandlers(services, types));
            messageMap = messageMap.Concat(LoadMessageHandlers(services, types));
        }

        services.Configure<TypeResolutionConfig>(c =>
        {
            c.MessageMap = messageMap.ToDictionary(kv => kv.Key, kv => kv.Value);
            c.RequestMap = requestMap.ToDictionary(kv => kv.Key, kv => kv.Value);
        });

        services.ConfigureOptions<ConfigurationBinder>();
    }

    public static void AddCommunicationMessageHandler(this IServiceCollection services, params Type[] markerTypes)
    {
        services.AddHostedService<HostedMessageService>();
    }

    public static void AddCommunicationRequestHandler(this IServiceCollection services, params Type[] markerTypes)
    {
        services.AddTransient<RequestMiddleware>();
    }

    private static Dictionary<string, Type> LoadRequestHandlers(IServiceCollection services, Type[] types)
    {
        var requestMap = new Dictionary<string, Type>();
        var handlers = types.Where(ImplementsOpenType(typeof(IRequestHandler<>), typeof(IRequestHandler<,>)));
        foreach (var handler in handlers)
        {
            var handlerInterface = handler.GetInterfaces().Single();
            var request = handlerInterface.GetGenericArguments()[0];
            var instance = Activator.CreateInstance(request) as IRequest;
            var requestType = instance!.RequestType;
            requestMap.Add(requestType, request);
            services.AddTransient(handlerInterface, handler);
        }

        return requestMap;
    }

    private static Dictionary<string, Type> LoadMessageHandlers(IServiceCollection services, Type[] types)
    {
        var messageMap = new Dictionary<string, Type>();
        var handlers = types.Where(ImplementsOpenType(typeof(IMessageHandler<>)));
        foreach (var handler in handlers)
        {
            var handlerInterface = handler.GetInterfaces().Single();
            var message = handlerInterface.GetGenericArguments()[0];
            var instance = Activator.CreateInstance(message) as IMessage;
            var messageType = instance!.MessageType;
            messageMap.Add(messageType, message);
            services.AddTransient(handlerInterface, handler);
        }

        return messageMap;
    }

    private static Func<Type, bool> ImplementsOpenType(params Type[] openTypes)
        => t => t.GetInterfaces().Intersect(openTypes, new TypeNameComparer()).Any();

    public static void UseCommunicationRequestMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestMiddleware>();
    }
}

internal class TypeNameComparer : IEqualityComparer<Type>
{
    public bool Equals(Type? x, Type? y) => x?.Name == y?.Name;

    public int GetHashCode([DisallowNull] Type obj) => obj.Name.GetHashCode();
}

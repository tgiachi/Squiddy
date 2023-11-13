using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using Squiddy.Core.Attributes.Services;
using Squiddy.Core.Interfaces.Events;
using Squiddy.Core.Services.Interfaces;

namespace Squiddy.Gui.Impl.Services;

[ServiceOrder(0)]
public class MessageBusService : IMessageBusService
{
    private readonly IServiceProvider _serviceProvider;

    public MessageBusService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Publishes a message to the message bus
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    public void Publish<T>(T message) where T : IBaseEvent
    {
        var publisher = _serviceProvider.GetRequiredService<IPublisher<T>>();

        publisher.Publish(message);
    }

    /// <summary>
    /// Publishes a message to the message bus asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public ValueTask PublishAsync<T>(T message) where T : IBaseEvent
    {
        var publisher = _serviceProvider.GetRequiredService<IAsyncPublisher<T>>();
        return publisher.PublishAsync(message);
    }

    /// <summary>
    ///  Subscribes to a message on the message bus
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    public void Subscribe<T>(Action<T> action) where T : IBaseEvent
    {
        var subscriber = _serviceProvider.GetRequiredService<ISubscriber<T>>();
        subscriber.Subscribe(action);
    }

    /// <summary>
    /// Publishes a message to the message bus asynchronously
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public ValueTask SubscribeAsync<T>(IAsyncMessageHandler<T> action) where T : IBaseEvent
    {
        var subscriber = _serviceProvider.GetRequiredService<IAsyncSubscriber<T>>();
        subscriber.Subscribe(action);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Requests a message response from the message bus asynchronously
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public ValueTask<TResponse> RequestAsync<TRequest, TResponse>(TRequest @event)
        where TRequest : IBaseEvent where TResponse : class
    {
        var requestHandler = _serviceProvider.GetRequiredService<IAsyncRequestHandler<TRequest, TResponse>>();
        return requestHandler.InvokeAsync(@event);
    }

    /// <summary>
    /// Requests a message response from the message bus
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public TResponse Request<TRequest, TResponse>(TRequest @event)
    {
        var requestHandler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return requestHandler.Invoke(@event);
    }

    public Task<bool> InitializeAsync() => Task.FromResult(true);

    public Task<bool> StartAsync() => Task.FromResult(true);

    public Task<bool> StopAsync() => Task.FromResult(true);
}

using MessagePipe;
using Squiddy.Core.Interfaces.Events;
using Squiddy.Core.Interfaces.Services;

namespace Squiddy.Core.Services.Interfaces;

/// <summary>
/// Interface for the message bus service
/// </summary>
public interface IMessageBusService : ISquiddyService
{
    void Publish<T>(T message) where T : IBaseEvent;
    ValueTask PublishAsync<T>(T message) where T : IBaseEvent;
    void Subscribe<T>(Action<T> action) where T : IBaseEvent;
    ValueTask SubscribeAsync<T>(IAsyncMessageHandler<T> action) where T : IBaseEvent;

    ValueTask<TResponse> RequestAsync<TRequest, TResponse>(TRequest @event)
        where TRequest : IBaseEvent where TResponse : class;

    TResponse Request<TRequest, TResponse>(TRequest @event);
}

using MessagePipe;
using Microsoft.Extensions.DependencyInjection;

namespace Squiddy.Core.MethodEx.Services;
public static class MessageBusServiceCollectionMethodEx
{
    /// <summary>
    /// Register MessageBus to Dependency Injection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterMessageBus(this IServiceCollection services)
    {
        return services.AddMessagePipe(
            options =>
            {
                options.InstanceLifetime = InstanceLifetime.Singleton;
                options.EnableCaptureStackTrace = true;
                options.DefaultAsyncPublishStrategy = AsyncPublishStrategy.Parallel;
            }
        );
    }
}

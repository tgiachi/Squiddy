using Microsoft.Extensions.DependencyInjection;

namespace Squiddy.Core.Interfaces.Bootstrap;

public interface ISquiddyBootstrap
{

    Task RunHostAsync(string[] args);

    Task ConfigureServices(Func<IServiceCollection, IServiceCollection> services);

    Task LoadServices(IServiceProvider serviceProvider, IServiceCollection serviceCollection);

    Task RunApplicationAsync(IServiceProvider serviceProvider);
}
